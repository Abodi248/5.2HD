using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.Artworks;

public class PatchDisplayStatusDto
{
    [Required]
    public bool IsOnDisplay { get; set; }
}
