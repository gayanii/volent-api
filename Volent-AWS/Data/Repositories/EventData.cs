using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volent_AWS.Common;
using Volent_AWS.Data;
using Volent_AWS.Models;

namespace Volent_AWS.Data.Repositories
{
    public class EventData : IEventData
    {
        private static AmazonDynamoDBClient amzonclient = new AmazonDynamoDBClient();
        public async Task CreateEvent(EventDTO eventDto)
        {
            string eventId = Guid.NewGuid().ToString();

            try
            {
                //Validate event

                //create pasword hash
                using (var client = new AmazonDynamoDBClient())
                {

                    //Add data to user table
                    await client.PutItemAsync(new PutItemRequest
                    {
                        TableName = "Events",
                        Item = new Dictionary<string, AttributeValue>
                        {
                            { "EventId", new AttributeValue { S = eventId}},
                            { "District", new AttributeValue { S =  ((int)eventDto.District).ToString()}},
                            { "Location", new AttributeValue { S = eventDto.Location}},
                            { "Description", new AttributeValue { S = eventDto.Description }},
                            { "EventBanner", new AttributeValue { S = eventDto.EventBanner }},
                            { "EventName", new AttributeValue { S = eventDto.EventName }},
                            { "EventStartDate", new AttributeValue { S = eventDto.EventStartDate.ToString() }},
                            { "EventEndDate", new AttributeValue { S = eventDto.EventEndDate.ToString() }},
                            { "EventStatus", new AttributeValue { S = ((int)eventDto.EventStatus).ToString() }}
                         }
                    });
                }

                //Add user interests to interests table
                using (var client = new AmazonDynamoDBClient())
                {
                    if (eventDto.Interests.Count != 0)
                    {
                        foreach (int interst in eventDto.Interests)
                        {
                            string interstId = Guid.NewGuid().ToString();
                            await client.PutItemAsync(new PutItemRequest
                            {
                                TableName = "EventInterests",
                                Item = new Dictionary<string, AttributeValue>
                                {
                                    { "Id" , new AttributeValue { S = interstId} },
                                    { "EventId", new AttributeValue { S = eventId}},
                                    { "InterestId", new AttributeValue { N =  interst.ToString()}},
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }
        }


        public async Task<List<EventDTO>> GetEvents(DisplayEventStatus type)
        {
         
            Console.WriteLine("Event Data -> Get Events: " + type.ToString());

            var events = new List<EventDTO>();
            var request = new ScanRequest { };
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                if (type == DisplayEventStatus.All)
                {
                    request = new ScanRequest
                    {
                        TableName = "Events",
                    };

                }
                else
                {
                    request = new ScanRequest
                    {
                        TableName = "Events",
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                            {":type", new AttributeValue {
                                 S = ((int)type).ToString()
                             }}
                        },
                        FilterExpression = "EventStatus=:type"
                    };
                }

                var response = client.ScanAsync(request);
             
                if (response.Result.Items.Count > 0)
                {
                    foreach (Dictionary<string, AttributeValue> item in response.Result.Items)
                    {
                        var eventItm = GetItem(item);

                        //EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(JsonConvert.SerializeObject(interest));

                        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss" };
                        var eventDTO = JsonConvert.DeserializeObject<EventDTO>(JsonConvert.SerializeObject(eventItm), dateTimeConverter);

                        events.Add(eventDTO);
                    }
                }

                return events;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }
        }

        public async Task<EventDTO> GetEventById(string eventId)
        {
            var eventDto = new EventDTO();

            Console.WriteLine("Get Event By Id -> " + eventId);

            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "Events",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":eventid", new AttributeValue {
                         S = eventId
                     }}
                },
                    FilterExpression = "EventId=:eventid"
                };

                var response = client.ScanAsync(request);

                if (response.Result.Items.Count > 0)
                {
                    var obj = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, AttributeValue> kvp in response.Result.Items[0])
                    {
                        string attributeName = kvp.Key;
                        AttributeValue value = kvp.Value;

                        var val = "";
                        if (value.S != null) { val = value.S; }
                        if (value.N != null) { val = value.N.ToString(); }

                        if (attributeName == "EventStartDate" || attributeName == "EventEndDate")
                        {
                            DateTime myDate = DateTime.Parse(value.S);
                            val = myDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }

                        obj.Add(attributeName, val);
                    }

                    eventDto = JsonConvert.DeserializeObject<EventDTO>(JsonConvert.SerializeObject(obj));

                    //Get event interest
                    List<int> userInterests = GetEventInterests(eventId);
                    eventDto.Interests = userInterests;
                }
 
               return eventDto;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }

    }

        private static Dictionary<string, string> GetItem(
            Dictionary<string, AttributeValue> attributeList)
        {
            var obj = new Dictionary<string, string>();
            foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
            {

                string attributeName = kvp.Key;
                AttributeValue value = kvp.Value;

                var val = "";
                if (value.S != null) { val = value.S; }
                if (value.N != null) { val = value.N; }

                if (attributeName == "EventStartDate" || attributeName == "EventEndDate")
                {
                    DateTime myDate = DateTime.Parse(value.S);

                    val = myDate.ToString("yyyy-MM-ddTHH:mm:ss");
                }

                obj.Add(attributeName, val);
            }

            return obj;
        }

        private List<int> GetEventInterests(string eventId)
        {
            var interestList = new List<int>();
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "EventInterests",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":eventid", new AttributeValue {
                         S = eventId
                     }}
                },
                    FilterExpression = "EventId=:eventid"
                };

                var response = client.ScanAsync(request);

                if (response.Result.Items.Count > 0)
                {
                    foreach (Dictionary<string, AttributeValue> item in response.Result.Items)
                    {
                        foreach (KeyValuePair<string, AttributeValue> kvp in item)
                        {

                            string attributeName = kvp.Key;
                            AttributeValue value = kvp.Value;

                            if (attributeName == "InterestId")
                            {
                                Console.WriteLine(attributeName + " " + value.N);
                                interestList.Add(int.Parse(value.N));
                            }

                        }
                    }
                }

                return interestList;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }
        }

        public async Task RateEvent(string userId, string eventId, RateDTO rateDTO)
        {

            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "UserEvents",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":eventid", new AttributeValue {
                         S = eventId
                    }},
                    {":userid", new AttributeValue {
                         S = userId
                    }}
                },
                    FilterExpression = "EventId=:eventid and UserId=:userid"
                };

                var response = client.ScanAsync(request);

                var val = "";
                if (response.Result.Items.Count > 0)
                {
                    var obj = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, AttributeValue> kvp in response.Result.Items[0])
                    {
                        string attributeName = kvp.Key;
                        AttributeValue value = kvp.Value;

                        if (attributeName == "Id")
                        {
                            val = value.S;
                        }

                    }
                }

                if (string.IsNullOrEmpty(val))
                {
                    throw new BadRequestException();
                }

                var updateRequest = new UpdateItemRequest
                {
                    TableName = "UserEvents",
                    Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { S = val } } },
                    ExpressionAttributeNames = new Dictionary<string, string>()
                    {
                        {"#rate", "EventRate"},
                        {"#comment", "EventComment"},
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    {
                        {":eventRate",new AttributeValue { S = rateDTO.Rate}},
                        {":eventCmmnt",new AttributeValue {S = rateDTO.Comment}}
                    },
                    UpdateExpression = "SET #rate=:eventRate, #comment=:eventCmmnt"
                };

                await client.UpdateItemAsync(updateRequest);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }


        }

    }
}
