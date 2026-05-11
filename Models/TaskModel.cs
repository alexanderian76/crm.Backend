using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace crm.Backend.Models
{
	public class TaskModel
	{
		public Guid Id { get; set; }
		[JsonPropertyName("assignedTo")]
		public Guid AssignedTo { get; set; }
		[JsonPropertyName("assignedToCompany")]
		public int? AssignedToCompany { get; set; }
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("title")]
		public string? Title { get; set; }
		[JsonPropertyName("type")]
		public int Type { get; set; }
		[JsonPropertyName("priority")]
		public int Priority { get; set; }
		[JsonPropertyName("state")]
		public int State { get; set; }
		[JsonPropertyName("executionDate")]
		public DateTime? ExecutionDate { get; set; }
		//public int[]? Tags { get; set; }
		[JsonPropertyName("location")]
		public string? Location { get; set; }



	}
}

