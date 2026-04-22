using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace EFstokTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void KategorileriGetir()
        {
            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                var kategoriler = db.Kategoriler.ToList();

                cmbKategori.DataSource = kategoriler;

                // Ekranda kullanıcının göreceği özellik (Sınıfındaki sütun adı)
                cmbKategori.DisplayMember = "KategoriAdi";

                // Seçim yapıldığında arka planda işlem görecek değer
                cmbKategori.ValueMember = "KategoriId";
            }
        }
        private  void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            KategorileriGetir();
        }
        private void Listele()
        {
            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                // Proxy oluşturmayı kapatırsan ilişkili tabloları kurcalayıp hata vermez
                db.Configuration.ProxyCreationEnabled = false;

                var liste = db.Urunler.ToList();
                dgvUrunler.DataSource = liste;
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                Urunler yeniUrun = new Urunler();

                yeniUrun.UrunAd = txtUrunAd.Text;
                yeniUrun.StokAdedi = (int)nmStok.Value;
                yeniUrun.Fiyat = decimal.Parse(txtFiyat.Text);

                // BURASI KRİTİK: SelectedValue, arka plandaki ID'yi (ValueMember) getirir.
                if (cmbKategori.SelectedValue != null)
                {
                    yeniUrun.KategoriId = (int)cmbKategori.SelectedValue;
                }

                db.Urunler.Add(yeniUrun);
                db.SaveChanges();

                MessageBox.Show("Ürün, seçilen kategori ID'si ile kaydedildi.");
                Listele();
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            
        
            int secilenId = int.Parse(dgvUrunler.CurrentRow.Cells["UrunId"].Value.ToString());

            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                var guncellenecekUrun = db.Urunler.Find(secilenId);
                if (guncellenecekUrun != null)
                {
                    guncellenecekUrun.UrunAd = txtUrunAd.Text;
                    guncellenecekUrun.StokAdedi = (int)nmStok.Value;
                    guncellenecekUrun.Fiyat = decimal.Parse(txtFiyat.Text);

                    db.SaveChanges();
                    MessageBox.Show("Ürün güncellendi.");
                    Listele();
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int secilenId = (int)dgvUrunler.CurrentRow.Cells["UrunId"].Value;

            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                var silinecekUrun = db.Urunler.Find(secilenId);
                if (silinecekUrun != null)
                {
                    db.Urunler.Remove(silinecekUrun);
                    db.SaveChanges();
                    MessageBox.Show("Ürün silindi.");
                    Listele();
                }
            }
        }

        private void btnKatagori_Click(object sender, EventArgs e)
        {
            KatagoriForm k = new KatagoriForm();
            k.ShowDialog();
        }
    }
    }
    
