using System.ComponentModel.DataAnnotations;
using BotCommon.Repository;

namespace DirectumCoffee;

public class UserSystemInfo
{
    [Key]
    public int Id { get; set; }
    public long UserId { get; set; }
    public BotUser BotUser { get; set; }
    public bool SearchDisable { get; set; }
    public bool PairFound { get; set; }
}