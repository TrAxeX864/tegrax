using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TgVozderzhansBot.Models
{
    public class PollItem
    {
        public long Id { get; set; }
        
        public User Owner { get; set; }
        
        [MaxLength(255)]
        public string Question { get; set; }
        
        [MaxLength(1500)]
        public string Options { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public int TotalUsers { get; set; }
        
        public int Position { get; set; }
        
        public DateTime? FinishedAt { get; set; }

        public void SetOptions(string[] options)
        {
            string str = string.Join(":::", options);

            Options = str;
        }

        public string[] GetOptions()
        {
            return Options.Split(":::");
        }
    }
}