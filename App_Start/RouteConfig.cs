using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRTool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Admin_TMS",
                url: "TMS",
                defaults: new { controller = "TMS", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_CaseLog",
                url: "Cases",
                defaults: new { controller = "AdminCaseLog", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_Training",
                url: "Training",
                defaults: new { controller = "AdminTraining", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_SkillEndrosment",
                url: "SkillEndrosment",
                defaults: new { controller = "AdminSkillEndrosment", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_Performance",
                url: "Performance",
                defaults: new { controller = "AdminPearformance", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_ProjectPlanner",
                url: "ProjectPlanner",
                defaults: new { controller = "AdminProjectPlanner", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_Planner",
                url: "Planner",
                defaults: new { controller = "AdminPlanner", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_Document",
                url: "Document",
                defaults: new { controller = "AdminDocument", action = "Index" }
            );

            routes.MapRoute(
                name: "Admin_News",
                url: "News",
                defaults: new { controller = "AdminNews", action = "Index" }
            );

            routes.MapRoute(
                name: "Me",
                url: "Me",
                defaults: new { controller = "Me", action = "Index" }
            );

            routes.MapRoute(
                name: "Me_ProjectPlanner",
                url: "Me/ProjectPlanner/{EmployeeId}",
                defaults: new { controller = "MeEmployeeProjectPlanner", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Me_MeEmployeeSkillsEndorsement",
                url: "Me/MeEmployeeSkillsEndorsement/{EmployeeId}",
                defaults: new { controller = "MeEmployeeSkillsEndorsement", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Me_Planner",
                url: "Me/Planner/{EmployeeId}",
                defaults: new { controller = "MeEmployeePlanner", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name:"Me_Performance",
                url: "Me/Performance/{EmployeeId}",
                defaults:new {controller= "MeEmployeePerformance", action= "MeEmployeePerformance", EmployeeId =UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Resources_Performance",
                url: "Resources/Performance/{EmployeeId}",
                defaults: new { controller = "EmployeePerformance", action = "EmployeePerformance", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_SkillsEndorsement",
                url: "Resources/SkillsEndorsement/{EmployeeId}",
                defaults: new { controller = "EmployeeSkillsEndorsement", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_Document",
                url: "Resources/Document/{EmployeeId}",
                defaults: new { controller = "EmployeeDocument", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_Profile",
                url: "Resources/Profile/{EmployeeId}",
                defaults: new { controller = "EmployeeProfile", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Resource_OverView",
              url: "Resource/Overview/{EmployeeId}",
              defaults: new { controller = "EmployeeOverView", action = "Index", EmployeeId = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "TMS_Vacancy",
                url: "TMSVacancy/VacancyDetail/{VacancyId}",
                defaults: new { controller = "TMSVacancy", action = "Index", VacancyId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_Planner",
                url: "Resources/Planner/{EmployeeId}",
                defaults: new { controller = "EmployeePlanner", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Resources_ProjectPlanner",
               url: "Resources/ProjectPlanner/{EmployeeId}",
               defaults: new { controller = "EmployeeProjectPlanner", action = "Index", EmployeeId = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Resources_Employment",
                url: "Resources/Employment/{EmployeeId}",
                defaults: new { controller = "EmployeeEmployment", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_ResumeCV_PDF",
                url: "Resources/ResumePDF/{EmployeeId}",
                defaults: new { controller = "EmployeeResume", action = "genaratePDF", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CoWorkerQueDetails",
               url: "EmployeePerformance/CoWorkerQueDetails/{PerCoworkerId}/{Id}",
               defaults: new { controller = "EmployeePerformance", action = "CoWorkerQueDetails", Id = UrlParameter.Optional},
               constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
           );
            routes.MapRoute(
               name: "CoWorkerQueDetailsForMe",
               url: "MeEmployeePerformance/CoWorkerQueDetailsForMe/{PerCoworkerId}/{Id}",
               defaults: new { controller = "MeEmployeePerformance", action = "CoWorkerQueDetailsForMe", Id = UrlParameter.Optional },
               constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
           );
            routes.MapRoute(
               name: "DeclineInvitation",
               url: "MeEmployeePerformance/DeclineInvitation/{PerCoworkerId}/{Id}",
               defaults: new { controller = "MeEmployeePerformance", action = "DeclineInvitation", Id = UrlParameter.Optional },
               constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
           );
            routes.MapRoute(
             name: "InviteCustomerDetails",
             url: "EmployeePerformance/InviteCustomerDetails/{EmpId}/{CustId}/{PerReviewId}/{Id}",
             defaults: new { controller = "EmployeePerformance", action = "InviteCustomerDetails", Id = UrlParameter.Optional },
             constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
         );

            routes.MapRoute(
                name: "Resources_ResumeCV",
                url: "Resources/ResumeCV/{EmployeeId}",
                defaults: new { controller = "EmployeeResume", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_Benefits",
                url: "Resources/Benefits/{EmployeeId}",
                defaults: new { controller = "EmployeeBenefits", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_Training",
                url: "Resources/Training/{EmployeeId}",
                defaults: new { controller = "EmployeeTraining", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_CaseLog",
                url: "Resources/CaseLog/{EmployeeId}",
                defaults: new { controller = "EmployeeCases", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources_Contacts",
                url: "Resources/Contacts/{EmployeeId}",
                defaults: new { controller = "EmployeeContacts", action = "Index", EmployeeId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Resources",
                url: "Resources",
                defaults: new { controller = "Employee", action = "Index" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Certificates",
                url: "Certificates",
                defaults: new { controller = "AdminCertificate", action = "Index" }
            );

            routes.MapRoute(
               name: "Visa",
               url: "Visa",
               defaults: new { controller = "AdminVisa", action = "Index" }
            );

            routes.MapRoute(
               name: "Tasks",
               url: "Tasks",
               defaults: new { controller = "AdminTask", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
