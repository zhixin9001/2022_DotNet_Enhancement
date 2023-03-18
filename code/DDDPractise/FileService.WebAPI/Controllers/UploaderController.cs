using FileService.Domain;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ZhiXin.ASPNETCore;

namespace FileService.WebAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
// [Authorize(Roles = "Admin")]
[UnitOfWork(typeof(FSDbContext))]
public class UploaderController : ControllerBase
{
    private readonly FSDomainService _domainService;
    private readonly FSDbContext _dbContext;
    private readonly FSRepository _repository;

    public UploaderController(FSDomainService domainService, FSDbContext dbContext, FSRepository repository)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadedRequest uploadedRequest,
        CancellationToken cancellationToken)
    {
        var file = uploadedRequest.File;
        var fileName = file.FileName;
        using var stream = file.OpenReadStream();
        var upItem = await _domainService.UploadAsync(stream, fileName, cancellationToken);
        _dbContext.Add(upItem);
        return upItem.RemoteUrl;
    }
}