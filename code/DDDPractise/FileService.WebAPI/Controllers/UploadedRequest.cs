using FluentValidation;

namespace FileService.WebAPI.Controllers;

public class UploadedRequest
{
    public IFormFile File { get; set; }
}

public class UploadedRequestValidator : AbstractValidator<UploadedRequest>
{
    public UploadedRequestValidator()
    {
        long maxFileSize = 50 * 1024 * 1024;
        RuleFor(a => a.File).NotNull().Must(f => f.Length > 0 && f.Length < maxFileSize);
    }
}