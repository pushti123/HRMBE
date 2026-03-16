using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;

namespace Helper
{
    public class CommonHelper
    {
        private readonly CommonRepositry _commonRepositry;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommonHelper(CommonRepositry commonRepositry, IHttpContextAccessor httpContextAccessor)
        {
            _commonRepositry = commonRepositry;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasPermission(string permission, int RoleId)
        {
            int permissionId =  _commonRepositry.PermissionList().Where(x => x.PermissionName.ToLower() == permission.ToLower()).Select(x => x.Id).FirstOrDefault();
            bool hasPermission =  _commonRepositry.RolePermissionList().Any(x => x.PermissionId == permissionId && x.RoleId == RoleId);
            return hasPermission;
        }

        public int GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");

            if (user == null)
                throw new UnauthorizedAccessException("User is not authenticated");

            int userId = int.Parse(user.Value);


            return userId;
        }

        public int GetRoleId()
        {
            var user = _httpContextAccessor.HttpContext?.User.FindFirst("RoleId");

            if (user == null)
                throw new UnauthorizedAccessException("User is not authenticated");
            int roleId = int.Parse(user.Value);

            return roleId;
        }

        public string ExtractText(string path)
        {
            if (path.EndsWith(".pdf"))
            {
                using var document = PdfDocument.Open(path);
                return string.Join(" ", document.GetPages().Select(p => p.Text));
            }

            if (path.EndsWith(".docx"))
            {
                using var doc = WordprocessingDocument.Open(path, false);
                return doc.MainDocumentPart.Document.Body.InnerText;
            }

            return "";
        }

    }
}
