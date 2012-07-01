using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Petanque.Web.Models
{
    public class TeamDto
    {
        public string Id { get; set; }
        public string Nom { get; set; }     
    }
}