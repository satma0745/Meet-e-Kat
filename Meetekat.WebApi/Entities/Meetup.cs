﻿namespace Meetekat.WebApi.Entities;

using System;
using System.Collections.Generic;
using Meetekat.WebApi.Entities.Users;

public class Meetup
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }
    
    public ICollection<string> Tags { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public Guid OrganizerId { get; set; }
    
    public Organizer Organizer { get; set; }
    
    public ICollection<Guest> SignedUpGuests { get; set; }
}
