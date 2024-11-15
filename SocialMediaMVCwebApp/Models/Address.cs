﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SocialMediaMVCwebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }// City/Village/street
        public string? Region{ get; set; } // Oblast, State...

    }
}
