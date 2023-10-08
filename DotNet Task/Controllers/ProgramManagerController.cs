using DAL;
using DAL.DTOs;
using DAL.Models;
using DAL.Models.ApplicationTemplate;
using DotNet_Task.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNet_Task.Controllers
{
    //unified controller to all tabs
    [Route("api/program_manager")]
    [ApiController]
    public class ProgramManagerController : ControllerBase
    {
        public ProgramManagerController(AppDbContext context, ICommonServices services, IConfiguration configuration)
        {
            _context = context;
            _services = services;
            _configuration = configuration;
        }
        private readonly AppDbContext _context;
        private readonly ICommonServices _services;
        private readonly IConfiguration _configuration;

        [HttpPost("SaveProgram")]
        public ActionResult SaveProgram(ProgramDetails details)
        {
            try
            {
                if (details == null)
                    return BadRequest("Invalid Data Provided");

                details.id = Guid.NewGuid().ToString();
                _context.ProgramDetails.Add(details);
                _context.SaveChanges();
                return Ok(new { id = details.id });
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        [HttpGet("GetProgram")]
        public ActionResult GetProgram([FromQuery] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(null);
                var program = _services.GetProgram(id);
                if (program == null)
                    return NotFound("Program Not Found");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPut("UpdateProgram")]
        public ActionResult UpdateProgram(ProgramDetails details)
        {
            try
            {
                if (details == null)
                    return BadRequest("Invalid Data Provided");
                var program = _services.GetProgram(details.id);
                if (program == null)
                    return BadRequest();

                program.title = details.title;
                program.type = details.type;
                program.summary = details.summary;
                program.description = details.description;
                program.skills = details.skills;
                program.benefits = details.benefits;
                program.criteria = details.criteria;
                program.location = details.location;
                program.duration = details.duration;
                program.isRemote = details.isRemote;
                program.maxApplicants = details.maxApplicants;
                program.minQualifications = details.minQualifications;
                program.programStart = details.programStart;
                program.applicationStart = details.applicationStart;
                program.applicationEnd = details.applicationEnd;

                _context.ProgramDetails.Update(program);
                _context.SaveChanges();
                return Ok(program);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        //Logic for Application form template

        [HttpPut("SaveTemplate")]
        public ActionResult SaveTemplate(ApplicationTemplate template, IFormFile image)
        {
            try
            {
                if (template == null)
                    return BadRequest("Invalid Data Provided");

                var program = _services.GetProgram(template.programId);
                if (program == null)
                    return BadRequest("Invalid program id");

                if (template.questions.Count() > 0)
                {
                    template.questions.ForEach(x =>
                    {
                        if (x.type != (byte?)Questions.questionType.dropDown && x.type != (byte?)Questions.questionType.multiChoice
                        && x.type != (byte?)Questions.questionType.yesNo)
                            x.choices = null;
                        else if (x.type == (byte?)Questions.questionType.yesNo)
                        {
                            x.choices = new QuestionsChoices();
                            x.choices.options.AddRange(new List<string> { "Yes", "No" });
                            x.choices.enableOther = false;
                        }
                    });
                }
                program.ApplicationTemplate = template;

                _context.ProgramDetails.Update(program);
                _context.SaveChanges();
                return Ok("Saved Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage(IFormFile image, [FromForm] string programId)
        {
            try
            {
                var program = _services.GetProgram(programId);
                if (program == null) return BadRequest("No programm Found");
                using var fileStream = image.OpenReadStream();
                var basePath = _configuration.GetSection("RootImageFolder").Value;
                var folderPath = Path.Combine(basePath, program.title + "__" + program.id);
                bool basePathExists = System.IO.Directory.Exists(folderPath);
                if (!basePathExists)
                    Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, program.id + Path.GetExtension(image.FileName).ToLower());
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var dirInfo = new DirectoryInfo(folderPath);
                    var files = dirInfo.GetFiles();
                    foreach ( var file in files)
                    {
                        string url = Request.Scheme + "://" + Request.Host + "/api/program_manager/download?title=" + program.title + "&id=" + program.id + "&name=" + file.Name;
                        program.imageUrl = url;
                    }
                    _context.ProgramDetails.Update(program);
                    _context.SaveChanges();
                    return Ok(new { imageUrl = string.Empty }); ;
                }
                return BadRequest("Error Occured");

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("downloadImage")]
        public ActionResult downloadImage([FromQuery] string title, [FromQuery] string id, [FromQuery] string name)
        {
            var basePath = _configuration.GetSection("RootImageFolder").Value;
            var fileInfo = new FileInfo(Path.Combine(basePath, title + "__" + id, name));

            if (!fileInfo.Exists)
            {
                return BadRequest("No file was found");
            }
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(fileInfo.Name);
            return File(fileInfo.OpenRead(), mimeType);
        }

        [HttpGet("GetTemplate")]
        public ActionResult GetTemplate([FromQuery] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Invalid ID");

                var program = _services.GetProgram(id);
                if (program == null)
                    return NotFound("No Program Found");

                return Ok(program.ApplicationTemplate);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        //Logic for workflow tab
        [HttpPut("SaveWorkflow")]
        public ActionResult SaveWorkflow(List<WorkflowStages> workflowStages)
        {
            try
            {
                if (workflowStages == null)
                    return BadRequest("Invalid Data Provided");

                var program = _services.GetProgram(workflowStages.FirstOrDefault().programId);
                if (program == null)
                    return BadRequest("Invalid program id");

                if (program.WorkflowStages.Count() > 0)
                {
                    //update old defined stages and remove it from the sent request
                    program.WorkflowStages.Where(x => workflowStages.Select(b => b.stageId).Contains(x.stageId)).ToList().ForEach(c =>
                    {
                        var x = workflowStages.FirstOrDefault(m => m.stageId.Equals(c.stageId));
                        c.stageName = x.stageName;
                        c.stageType = x.stageType;
                        c.questions = x.questions;
                        c.isShownToCandidate = x.isShownToCandidate;
                        workflowStages.Remove(x);
                    });

                    //Add all new stages
                    foreach (var stage in workflowStages)
                    {
                        stage.stageId = Guid.NewGuid().ToString();
                    }
                    program.WorkflowStages.AddRange(workflowStages);
                }
                else
                {
                    foreach (var stage in program.WorkflowStages)
                    {
                        stage.stageId = Guid.NewGuid().ToString();
                    }
                    program.WorkflowStages.AddRange(workflowStages);
                }
                _context.ProgramDetails.Update(program);
                _context.SaveChanges();
                return Ok("Saved Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }


        [HttpGet("GetWorkflow")]
        public ActionResult GetWorkflow([FromQuery] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Invalid ID");

                var program = _services.GetProgram(id);
                if (program == null)
                    return NotFound("No Program Found");

                return Ok(program.WorkflowStages);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        //Logic For Preview Tab

        [HttpGet("GetPreview")]
        public ActionResult GetPreview([FromQuery] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Invalid ID");

                var program = _services.GetProgram(id);
                if (program == null)
                    return NotFound("No Program Found");
                /*Convert Program object to json and then deseralize to an object
                 of summary DTO type
                */
                var programJson = JsonConvert.SerializeObject(program);
                var summary = JsonConvert.DeserializeObject<SummaryPreviewDTO>(programJson);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
    }
}
