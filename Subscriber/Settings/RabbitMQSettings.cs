using System.ComponentModel.DataAnnotations;

public class RabbitMQSettings
{
    [Required]
    [StringLength(100)]
    public string HostName { get; set; }

    [Required]
    [Range(1, 65535)]
    public int Port { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; }

    [Required]
    [StringLength(50)]
    public string Password { get; set; }

    [Required]
    [StringLength(50)]
    public string Exchange { get; set; }

    [Required]
    [StringLength(50)]
    public string RoutingKey { get; set; }
}
