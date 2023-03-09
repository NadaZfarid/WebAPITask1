using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebAPIDay1.DTO;
using WebAPIDay1.Models;

namespace WebAPIDay1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        ApiDbContext DB;
        public CoursesController(ApiDbContext DB)
        {
            this.DB = DB;
        }
        [HttpGet]
        
        public ActionResult getAll()
        {
            List <Course>courses = DB.Courses.Include(c=>c.Top).ToList();
            List<CoursesDTO> coursesDTOs= new List<CoursesDTO>();
            foreach(Course course in courses) 
            {
                CoursesDTO dTO = new CoursesDTO()
                {
                    ID = course.ID,
                    Crs_name = course.Crs_name,
                    Crs_des = course.Crs_des,
                    Duration = course.Duration,
                    Top_Name = course.Top.Top_Name,
                    Top_ID = course.Top_Id
                    
                };
                coursesDTOs.Add(dTO);
            };
            if (coursesDTOs == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(coursesDTOs);
            }
        }

        [HttpGet("/api/Topic")]
        public ActionResult getTopics()
        {
            List<Topic> topics = DB.Topics.ToList();
            List<TopicDTO> topicDTOs = new List<TopicDTO>();
            foreach (Topic topic in topics)
            {
                TopicDTO dTO = new TopicDTO()
                {
                    Top_ID = topic.Top_ID,
                    Top_Name = topic.Top_Name
                };
                topicDTOs.Add(dTO);
            };
            if (topicDTOs == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(topicDTOs);
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult getById(int id)
        {
            Course course = DB.Courses.Include(c=>c.Top).Where(c=>c.ID==id).SingleOrDefault();
            CoursesDTO dTO = new CoursesDTO()
            {
                ID = course.ID,
                Crs_name=course.Crs_name,
                Crs_des=course.Crs_des,
                Duration= course.Duration,
                Top_Name= course.Top.Top_Name
            };
            if (dTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dTO);
            }
        }

        [HttpDelete]
        public ActionResult deleteCourse(int id)
        {
            var course = DB.Courses.Where(c => c.ID == id).SingleOrDefault();
            if (course == null)
            {
                return NotFound();
            }
            else
            {
                DB.Courses.Remove(course);
                DB.SaveChanges();
                return getAll();
            }
        }
        [HttpPost]
        public ActionResult post(CoursesDTO courseDTO)
        {
            if (courseDTO == null)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    Course course = new Course()
                    {
                        Crs_name = courseDTO.Crs_name,
                        Crs_des = courseDTO.Crs_des,
                        Duration = courseDTO.Duration,
                        Top_Id = courseDTO.Top_ID
                    };
                    DB.Courses.Add(course);
                    DB.SaveChanges();
                    return Created("url", courseDTO);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut]
        public ActionResult put(int id, CoursesDTO courseDTO)
        {
            if (id != courseDTO.ID)
            {
                return BadRequest();
            }
            else if (!(DB.Courses.Where(c=>c.ID==id).Any()))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    Course course = new Course()
                    {
                        ID = courseDTO.ID,
                        Crs_name = courseDTO.Crs_name,
                        Crs_des = courseDTO.Crs_des,
                        Duration = courseDTO.Duration,
                        Top_Id = courseDTO.Top_ID
                    };
                    DB.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    DB.SaveChanges();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }
        [HttpGet("{name:alpha}")]
        public ActionResult couseByName(string name)
        {
            Course course = DB.Courses.Include(c=>c.Top).Where(c => c.Crs_name.Equals( name)).SingleOrDefault();
            
            if (course == null)
            {
                return NotFound();
            }
            else
            {
                CoursesDTO dTO = new CoursesDTO()
                {
                    ID = course.ID,
                    Crs_name = course.Crs_name,
                    Crs_des = course.Crs_des,
                    Duration = course.Duration,
                    Top_Name = course.Top.Top_Name
                };
                return Ok(dTO);
            }
        }
    }
}
