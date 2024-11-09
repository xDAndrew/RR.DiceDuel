using System.ComponentModel.DataAnnotations;

namespace RR.DiceDuel.ExternalServices.EntityFramework.Entities;

public class ConfigEntity
{
    [Key]
    public int Id { get; set; }
    
    public int RoomMaxPlayer { get; set; }
    
    public int MaxGameRound { get; set; }
}