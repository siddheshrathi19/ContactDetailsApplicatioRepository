using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactApplication.Areas.Contact.Models;

namespace ContactApplication.Controllers
{
    /// <summary>
    /// Common route prefix api/Contact
    /// </summary>
    /// api/Contact/GetContact
    [RoutePrefix("api/Contact")]
    public class ContactController : ApiController
    {
        /// <summary>
        /// Function to get all Contacts
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        [Route("GetContact")]
        public IHttpActionResult GetContact()
        {
            IList<ContactDetail> contactsList = null;
            using (ContactDBEntities dbContext = new ContactDBEntities())
            {
                contactsList = dbContext.ContactDetails.ToList();
            }
            if (contactsList.Count == 0)
            {
                return NotFound();
            }
            return Ok(contactsList);
        }


        /// <summary>
        /// Function to get Contact by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        /// api/Contact/GetContactById/5
        [HttpGet]
        [Route("GetContactById/{id}")]
        public IHttpActionResult GetContactById(int id)
        {
            ContactDetail contact = null;
            using (ContactDBEntities dbContext = new ContactDBEntities())
            {
                contact = dbContext.ContactDetails.FirstOrDefault(c => c.ID.Equals(id));
            }
            if (contact == null)
                return NotFound();
            else
                return Ok(contact);
        }


        /// <summary>
        /// Function to add a new Contact
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>HttpResponseMessage</returns>
        /// api/Contact/CreateContact
        [HttpPost]
        [Route("CreateContact")]
        public HttpResponseMessage CreateContact([FromBody]ContactDetail requestData)
        {
            try
            {
                ContactDetail contact = requestData;
                using (ContactDBEntities dbContext = new ContactDBEntities())
                {
                    dbContext.ContactDetails.Add(contact);
                    dbContext.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.Created, contact);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }
        }


        /// <summary>
        /// Function to update a Contact
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestData"></param>
        /// <returns>HttpResponseMessage</returns>
        /// api/Contact/CreateContact/5
        [HttpPut]
        [Route("UpdateContact")]
        public HttpResponseMessage UpdateContact(int id, [FromBody]ContactDetail requestData)
        {
            try
            {
                ContactDetail contactDetail = requestData;
                using (ContactDBEntities dbContext = new ContactDBEntities())
                {
                    ContactDetail contact = dbContext.ContactDetails.FirstOrDefault(x => x.ID.Equals(id));
                    if (contact == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact Details with id = " + id.ToString() + "not found");
                    }
                    else
                    {
                        contact.FirstName = contactDetail.FirstName;
                        contact.LastName = contactDetail.LastName;
                        contact.Email = contactDetail.Email;
                        contact.PhoneNumber = contactDetail.PhoneNumber;
                        contact.Status = contactDetail.Status;
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, contact);
                    }
                }
            }
            catch (Exception exception)
            {
                //log exception
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }
        }

        /// <summary>
        /// Function to delete a Contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HttpResponseMessage</returns>
        /// api/Contact/DeleteContact/5
        [HttpDelete]
        [Route("DeleteContact/{id}")]
        public HttpResponseMessage DeleteContact(int id)
        {
            using (ContactDBEntities dbContext = new ContactDBEntities())
            {
                try
                {
                    ContactDetail contact = dbContext.ContactDetails.FirstOrDefault(c => c.ID.Equals(id));
                    if (contact == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact with id = " + id.ToString() + "not found");
                    }
                    else
                    {
                        dbContext.ContactDetails.Remove(contact);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch (Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Ex);
                }
            }
        }
    }
}