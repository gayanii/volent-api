using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volent_AWS.Common
{
    public enum UserStatus
    {
        Active = 1,
        Deactive
    }

    public enum District
    {
        Ampara = 1,
        Anuradhapura,
        Badulla,
        Batticaloa,
        Colombo,
        Galle,
        Gampaha,
        Hambantota,
        Jaffna,
        Kalutara,
        Kandy,
        Kegalle,
        Kilinochchi,
        Kurunegala,
        Mannar,
        Matale,
        Matara,
        Moneragala,
        Mullaitivu,
        NuwaraEliya,
        Polonnaruwa,
        Puttalam,
        Ratnapura,
        Trincomalee,
        Vavuniya
    }

    public enum Profession
    {
        accountant = 1,
        actor,
        architect,
        artist,
        attorney,
        banker,
        cashier,
        coach,
        designer,
        doctor,
        electrician,
        engineer,
        filmmaker,
        lawyer,
        mechanic,
        musician,
        nurse,
        painter,
        pharmacist,
        photographer,
        physician,
        pilot,
        plumber,
        police,
        politician,
        professor,
        programmer,
        psychologist,
        receptionist,
        secretary,
        singer,
        surgeon,
        teacher,
        therapist,
        translator,
        veterinarian,
        videographer,
        writer,
        other
    }

    public enum EventStatus
    {
        Draft=1,
        Published,
        Expired,
        Blocked
    }

    public enum DisplayEventStatus
    {
        All = 0,
        Draft = 1,
        Published,
        Expired,
        Blocked
    }

    public enum NotificationType
    {
        All = 0,
        NearMe = 1,
        Interested
    }
}
