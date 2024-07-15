using System.ComponentModel.DataAnnotations;
using BotCommon.Repository;

namespace DirectumCoffee;

public class UserInfo
{
    [Key]
    public int Id { get; set; }
    public long UserId { get; set; }
    public BotUser BotUser { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Work { get; set; }
    public string Hobby { get; set; }
    public string Interests { get; set; }
}