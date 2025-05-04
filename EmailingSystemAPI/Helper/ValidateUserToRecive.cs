using EmailingSystem.Core.Entities;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;

namespace EmailingSystemAPI.Helper
{
    public static class ValidateUserToRecive
    {


        public static async Task<bool> IsUserValidToRecieve(this UserManager<ApplicationUser> userManager, ApplicationUser Sender , ApplicationUser Reciver)
        {
            var SenderRole = (await userManager.GetRolesAsync(Sender)).FirstOrDefault();

            var ReciverRole = (await userManager.GetRolesAsync(Reciver)).FirstOrDefault();
            if (Sender.Id != Reciver.Id)
            {

                if (SenderRole == "Admin" || SenderRole == "Presedient")
                {
                    return true;

                }


                else if (SenderRole == "CollegeAdmin")
                {
                    if (Reciver.CollegeId == Sender.CollegeId)
                    {
                        return true;
                    }
                }

                else if (SenderRole == "Dean") 
                {
                    if (ReciverRole == "Presedient"
                        || ReciverRole == "Secretary"
                        || ReciverRole == "Dean"
                        || Sender.CollegeId == Reciver.CollegeId
                        )
                    { return true; }

                }

                else if (SenderRole == "VicePresedientForEnvironment")
                {
                    if (ReciverRole == "Presedient" || ReciverRole == "ViceDeanForEnvironment")
                    {
                        return true;
                    }

                }

                else if (SenderRole == "VicePresedientForStudentsAffairs")
                {
                    if (ReciverRole == "Presedient" || ReciverRole == "ViceDeanForStudentsAffairs")
                    {
                        return true;
                    }

                }

                else if (SenderRole == "VicePresedientForPostgraduatStudies")
                {
                    if (ReciverRole == "Presedient" || ReciverRole == "ViceDeanForPostgraduatStudies")
                    {
                        return true;
                    }

                }
                else if (SenderRole == "NormalUser")
                {
                    if (Reciver.CollegeId == Sender.CollegeId && ReciverRole == "Secretary")
                    {
                        return true;
                    }
                }

                else if (SenderRole == "Secretary")
                {
                    if (Sender.CollegeId == Reciver.CollegeId)
                    {
                        return true;
                    }
                    else if (ReciverRole == "Dean")
                    {
                        return true;
                    }
                }
            }


            return false;


        }



    }
}
