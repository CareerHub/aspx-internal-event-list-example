using System;
using System.Collections.Generic;

namespace Event_List_Example.Models {
    public enum EventBookingType {
        NotRequired = 0,
        SelfManaged = 1,
        AdminManaged = 2,
        External = 3,
        ExternalWebsite = 4
    }
    public class EventModel {
        public int EntityID { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime? Publish { get; set; }
        public DateTime? Expire { get; set; }

        public bool IsActive { get; set; }
        public bool IsCancelled { get; set; }

        public string Summary { get; set; }
        public string Details { get; set; }

        public string OffCampusVenue { get; set; }
        public string Campus { get; set; }
        public string Building { get; set; }
        public string Location { get; set; }
        public EventBookingType BookingType { get; set; }
    }
}