using AdaYazilimTrenRezervasyonUygulamasi.Models.RequestClass;
using AdaYazilimTrenRezervasyonUygulamasi.Models.ResponseClass;
using Microsoft.AspNetCore.Mvc;

namespace AdaYazilimTrenRezervasyonUygulamasi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : Controller
    {
        [HttpPost]
        public IActionResult Rezervasyon(RezervasyonTalebi rezervasyonTalebi)
        {
            var vagonListesi = rezervasyonTalebi.Tren.Vagonlar.ToList();
            var yanit = new Yanıt();

            if (rezervasyonTalebi.KisilerFarkliVagonlaraYerlestirilebilir == false)
            {
                foreach (var vagon in vagonListesi)
                {
                    if ((vagon.DoluKoltukAdet < vagon.Kapasite * 0.7) && ((rezervasyonTalebi.RezervasyonYapilacakKisiSayisi + vagon.DoluKoltukAdet) <= vagon.Kapasite))
                    {
                        int makYerlestirilebilirKisiAdeti = (int)Math.Round(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;
                        if (makYerlestirilebilirKisiAdeti >= rezervasyonTalebi.RezervasyonYapilacakKisiSayisi)
                        {
                            yanit.YerlesimAyrinti.Add(new YerlesimAyrinti
                            {
                                KisiSayisi = rezervasyonTalebi.RezervasyonYapilacakKisiSayisi,
                                VagonAdi = vagon.Ad
                            });//burada 2. vagona yerleşecek
                            break;
                        }
                    }
                }
            }
            else
            {
                int toplamBosYer = 0;
                foreach (var vagon in vagonListesi)
                {
                    toplamBosYer += vagon.Kapasite - vagon.DoluKoltukAdet;
                }
                if (rezervasyonTalebi.RezervasyonYapilacakKisiSayisi > toplamBosYer)
                {
                    return Json(new Yanıt
                    {
                        RezervasyonYapilabilir = false
                    });
                }

                foreach (var vagon in vagonListesi)
                {
                    //  && (rezervasyonTalebi.RezervasyonYapilacakKisiSayisi + vagon.DoluKoltukAdet) <= vagon.Kapasite
                    int makYerlestirilebilirKisiAdeti = (int)Math.Round(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;
                    if (vagon.DoluKoltukAdet <= vagon.Kapasite * 0.7)
                    {
                        if (rezervasyonTalebi.RezervasyonYapilacakKisiSayisi <= makYerlestirilebilirKisiAdeti)
                        {
                            yanit.YerlesimAyrinti.Add(new YerlesimAyrinti
                            {
                                VagonAdi = vagon.Ad,
                                KisiSayisi = rezervasyonTalebi.RezervasyonYapilacakKisiSayisi
                            });
                            rezervasyonTalebi.RezervasyonYapilacakKisiSayisi = 0;
                            break;
                        }
                        else
                        {
                            int kalanKisiSayisi = rezervasyonTalebi.RezervasyonYapilacakKisiSayisi - makYerlestirilebilirKisiAdeti;
                            yanit.YerlesimAyrinti.Add(new YerlesimAyrinti
                            {
                                VagonAdi = vagon.Ad,
                                KisiSayisi = makYerlestirilebilirKisiAdeti
                            });

                            rezervasyonTalebi.RezervasyonYapilacakKisiSayisi = kalanKisiSayisi;
                        }


                    }

                }

                if (rezervasyonTalebi.RezervasyonYapilacakKisiSayisi != 0)
                {
                    return Ok(new Yanıt
                    {
                        RezervasyonYapilabilir = false
                    });
                }

            }

            if (yanit.YerlesimAyrinti.Count == 0)
            {
                yanit.RezervasyonYapilabilir = false;
            }
            else
            {
                yanit.RezervasyonYapilabilir = true;
            }

            return Ok(yanit);
        }
    }
}
