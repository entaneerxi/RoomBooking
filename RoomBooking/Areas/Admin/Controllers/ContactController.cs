using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
            var socialMedia = await _context.SocialMedias.OrderBy(s => s.DisplayOrder).ToListAsync();

            ViewBag.SocialMedia = socialMedia;
            return View(contactInfo);
        }

        public async Task<IActionResult> EditContactInfo(int? id)
        {
            ContactInfo? contactInfo;
            if (id == null)
            {
                contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
            }
            else
            {
                contactInfo = await _context.ContactInfos.FindAsync(id);
            }

            if (contactInfo == null)
            {
                contactInfo = new ContactInfo();
            }

            return View(contactInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContactInfo([Bind("Id,PropertyName,Address,Phone,Email,Website,MapEmbedUrl,Description,CreatedAt")] ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                if (contactInfo.Id == 0)
                {
                    contactInfo.CreatedAt = DateTime.UtcNow;
                    _context.Add(contactInfo);
                }
                else
                {
                    contactInfo.UpdatedAt = DateTime.UtcNow;
                    _context.Update(contactInfo);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactInfo);
        }

        public IActionResult CreateSocialMedia()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSocialMedia([Bind("Id,Platform,Url,IconClass,IsActive,DisplayOrder")] SocialMedia socialMedia)
        {
            if (ModelState.IsValid)
            {
                socialMedia.CreatedAt = DateTime.UtcNow;
                _context.Add(socialMedia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(socialMedia);
        }

        public async Task<IActionResult> EditSocialMedia(int? id)
        {
            if (id == null) return NotFound();
            var socialMedia = await _context.SocialMedias.FindAsync(id);
            if (socialMedia == null) return NotFound();
            return View(socialMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSocialMedia(int id, [Bind("Id,Platform,Url,IconClass,IsActive,DisplayOrder,CreatedAt")] SocialMedia socialMedia)
        {
            if (id != socialMedia.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialMedia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialMediaExists(socialMedia.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(socialMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSocialMedia(int id)
        {
            var socialMedia = await _context.SocialMedias.FindAsync(id);
            if (socialMedia != null)
            {
                _context.SocialMedias.Remove(socialMedia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SocialMediaExists(int id)
        {
            return _context.SocialMedias.Any(e => e.Id == id);
        }
    }
}
