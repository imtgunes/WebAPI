using Microsoft.AspNetCore.Mvc;
using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private string publicKey = "<RSAKeyValue><Modulus>sU7SHN6d/A0cuBDYxnbJRm3cwwFcQotrwRqXBpaLLxE+/xZqoBP8buMAXUf3jJKDVmkiCFV4lA7OlvX2ENw0yubsCYwYJfce8/qbfijl7bir3Jhf669gGuFqB5awQU+hccciU12D+fJfkn0Uak/j2QLkhWQ8puTBe/IuOB3/y1k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        ContextToDo contextToDo = new ContextToDo();

        [HttpGet("Users/{userMail}&{userPassword}")]
        public IEnumerable<Users> Get(string userMail, string userPassword)
        {
            Decryption decryption = new Decryption();
            int userID = contextToDo.Users.Where(u => u.userMail == userMail).Select(u => u.userID).SingleOrDefault();
            if(userID == 0)
            {
                return Enumerable.Empty<Users>();
            }
            else
            {
                string encryptedPassword = contextToDo.Users.Where(u => u.userMail == userMail).Select(u => u.userPassword).SingleOrDefault().ToString();
                var decryptedPassword = decryption.RSADecrypt(encryptedPassword, false);
                
                if (decryptedPassword == userPassword)
                {
                    return contextToDo.Users.Where(u => u.userID == userID);
                }
                else
                {
                    return Enumerable.Empty<Users>();
                }
            }
            
            
        }

        [HttpGet("User/{userID}")]
        public IEnumerable<Users> GetUserInfo(int userID)
        {
            return contextToDo.Users.Where(u => u.userID == userID).Select(u => new { u.userID, u.userName, u.userSurName, u.userMail, u.userProfileImage }).ToList().Select(x => new Users()
            {
                userID = x.userID,
                userName = x.userName,
                userSurName = x.userSurName,
                userProfileImage = x.userProfileImage
            });

        }

        [HttpPost("Users/User")]
        public IEnumerable<Users> GetUser([FromBody] Users user)
        {
            Decryption decryption = new Decryption();
            int userID = contextToDo.Users.Where(u => u.userMail == user.userMail).Select(u => u.userID).SingleOrDefault();
            if (userID == 0)
            {
                return Enumerable.Empty<Users>();
            }
            else
            {
                string encryptedPassword = contextToDo.Users.Where(u => u.userMail == user.userMail).Select(u => u.userPassword).SingleOrDefault().ToString();
                var decryptedPassword = decryption.RSADecrypt(encryptedPassword, false);
                
                if (decryptedPassword == user.userPassword)
                {
                    contextToDo.Users.Find(userID).userLogin = DateTime.Now.AddYears(1);
                    contextToDo.SaveChanges();
                    return contextToDo.Users.Where(u => u.userID == userID).Select(u => new { u.userID, u.userName, u.userSurName ,u.userMail,u.userProfileImage}).ToList().Select(x => new Users()
                    {
                        userID = x.userID,
                        userName = x.userName,
                        userSurName = x.userSurName,
                        userProfileImage = x.userProfileImage
                    });
                }
                else
                {
                    return Enumerable.Empty<Users>();
                }
            }


        }

        [HttpPost("Users/LoginTimeOut")]
        public int GetIsLogin([FromBody] Users user)
        {
            int status = 0;
            var datetimeLogin = contextToDo.Users.Where(u => u.userID == user.userID).Select(u => u.userLogin).SingleOrDefault();
            
            if (datetimeLogin == null)
            {
                status = 0;
            }
            else
            {
                var datetimeNow = DateTime.Now;
                TimeSpan differenceDate = (TimeSpan)(datetimeLogin - datetimeNow);
                int differenceInDays = differenceDate.Days;
                int differenceInHours = differenceDate.Hours;
                int differenceInMinutes = differenceDate.Minutes;

                if (differenceInDays < 1)
                {
                    if(differenceInHours < 1)
                    {
                        if(differenceInMinutes < 1)
                        {
                            status = 0;
                        }
                        else
                        {
                            status = 1;
                        }
                    }
                    else
                    {
                        status = 1;
                    }

                }
                else
                {
                    status = 1;
                }
            }
            return status;
        }


        [HttpPost("Users")]
        public int Post([FromBody] Users users)
        {
            int addedUser = 0;
           
            if (contextToDo.Users.Where(u => u.userMail == users.userMail).Select(u => u.userMail).SingleOrDefault() == null)
            {
                try
                {
                    Encryption encryption = new Encryption();
                    users.userPassword = encryption.RSAEncrypt(users.userPassword, publicKey, false);
                    contextToDo.Users.Add(users);
                    contextToDo.SaveChanges();
                    addedUser = 1;
                }
                catch (Exception e)
                {
                    addedUser = 0;
                }
            }
            else
            {

                addedUser = 2;
            }
            
            return addedUser;
        }

    }
}
