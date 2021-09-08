using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_API.Controllers
{
    public class EmployeeController : ApiController
    {
        /// <summary>  
        /// Filter out epmloyee whose details are asked   
        /// </summary>  
        /// <param name="id">id of employee</param>  
        /// <returns>employee details of id</returns>  
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            try
            {
                using (EmployeeManagementEntities entities = new EmployeeManagementEntities())
                {
                    var emp = entities.Employees.FirstOrDefault(em => em.Id == id);
                    if (emp != null)
                    {
                        return Ok(emp);
                    }
                    else
                    {
                        return Content(HttpStatusCode.NotFound, "Employee with Id: " + id + " not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);

            }
        }

        /// <summary>  
        /// Creates a new employee  
        /// </summary>  
        /// <param name="employee"></param>  
        /// <returns>details of newly created employee</returns>  
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeManagementEntities entities = new EmployeeManagementEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var res = Request.CreateResponse(HttpStatusCode.Created, employee);
                    res.Headers.Location = new Uri(Request.RequestUri + employee.Id.ToString());
                    return res;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>  
        /// Update details of employee based on id  
        /// </summary>  
        /// <param name="id"></param>  
        /// <param name="emp"></param>  
        /// <returns>updated details of employee</returns>  
        [HttpPut]
        public HttpResponseMessage Put(string id, [FromBody] Employee emp)
        {
            try
            {
                using (EmployeeManagementEntities entities = new EmployeeManagementEntities())
                {
                    var employee = entities.Employees.Where(em => em.Id == id).FirstOrDefault();
                    if (employee != null)
                    {
                        if (!string.IsNullOrWhiteSpace(emp.name))
                            employee.name = emp.name;

                        if (!string.IsNullOrWhiteSpace(emp.email))
                            employee.email = emp.email;

                        if (!string.IsNullOrWhiteSpace(emp.gender))
                            employee.gender = emp.gender;

                        if (!string.IsNullOrWhiteSpace(emp.status))
                            employee.status = emp.status;

                        entities.SaveChanges();
                        var res = Request.CreateResponse(HttpStatusCode.OK, "Employee with id" + id + " updated");
                        res.Headers.Location = new Uri(Request.RequestUri + id.ToString());
                        return res;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id" + id + " is not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>  
        /// Deletes the respected employee based on id passed.  
        /// </summary>  
        /// <param name="id"></param>  
        /// <returns>id of deleted employee</returns>  
        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                using (EmployeeManagementEntities entities = new EmployeeManagementEntities())
                {
                    var employee = entities.Employees.Where(emp => emp.Id == id).FirstOrDefault();
                    if (employee != null)
                    {
                        entities.Employees.Remove(employee);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Employee with id" + id + " Deleted");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id" + id + " is not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
