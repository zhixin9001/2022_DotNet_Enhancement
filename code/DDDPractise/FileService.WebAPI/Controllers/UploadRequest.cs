﻿namespace FileService.WebAPI.Controllers;

public class UploadRequest
{
    //不要声明为Action的参数，否则不会正常工作
    public IFormFile File { get; set; }
}