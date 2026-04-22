using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFstokTakip
{
    public partial class KatagoriForm : Form
    {
        public KatagoriForm()
        {
            InitializeComponent();
        }

        private void KatagoriForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
        private void Listele()
        {
            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                db.Configuration.ProxyCreationEnabled = false;
                dgvKategoriler.DataSource = db.Kategoriler.ToList();
            }
        }
        private void btnListele_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKatoAd.Text)) return;

            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                Kategoriler yeniKat = new Kategoriler
                {
                    KategoriAdi = txtKatoAd.Text
                };

                db.Kategoriler.Add(yeniKat);
                db.SaveChanges();

                txtKatoAd.Clear();
                Listele();
                MessageBox.Show("Yeni kategori eklendi.");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dgvKategoriler.CurrentRow == null) return;

            int id = (int)dgvKategoriler.CurrentRow.Cells["KategoriId"].Value;

            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                var guncellenecek = db.Kategoriler.FirstOrDefault(x => x.KategoriId == id);
                if (guncellenecek != null)
                {
                    guncellenecek.KategoriAdi = txtKatoAd.Text;
                    db.SaveChanges();
                    Listele();
                    MessageBox.Show("Kategori adı güncellendi.");
                }
            }
        }
        
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvKategoriler.CurrentRow == null) return;

            int id = (int)dgvKategoriler.CurrentRow.Cells["KategoriId"].Value;

            using (var db = new StokTakipDbEntities(VeritabaniAyarlari.GetEfBaglantiMetni()))
            {
                var silinecek = db.Kategoriler.Find(id);
                if (silinecek != null)
                {
                    db.Kategoriler.Remove(silinecek);
                    db.SaveChanges();
                    Listele();
                    MessageBox.Show("Kategori silindi.");
                }
            }
        }
    }
}
