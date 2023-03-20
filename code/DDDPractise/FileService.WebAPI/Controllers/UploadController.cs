using FileService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FileService.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UploadController : ControllerBase
{
    private readonly IFSRepository _fsRepository;
    private readonly FSDomainService _domainService;

    public UploadController(IFSRepository fsRepository, FSDomainService domainService)
    {
        _fsRepository = fsRepository;
        _domainService = domainService;
    }

    [HttpGet]
    public async Task<ActionResult<bool>> GetFileExist(string fileName)
    {
        var item = await _fsRepository.FindFileAsync(fileName);
        return item != null;
    }

    [HttpPost]
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest request, CancellationToken token)
    {
        var file = request.File;
        var fileName = file.FileName;
        using var stream = file.OpenReadStream();
        var uploadedItem = await _domainService.UploadAsync(stream, fileName, token);
        return uploadedItem.RemoteUrl;
    }
}