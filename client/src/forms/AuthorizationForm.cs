using System.Collections;
using System.Media;
using System.Resources;
using System.Text;
using client.bindings;
using client.Properties;
using LLibrary.Guards;

namespace client.src.forms
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void AuthorizationForm_Load(object sender, EventArgs e) { }

        private void AutoFillButton_Click(object sender, EventArgs e)
        {
            AuthorizationBinding bind = Guard.AgainstNull(
                authorizationBindingBindingSource.Current as AuthorizationBinding
            );
            Random r = new Random();
            bind.Username =
                _randomUsernames[r.Next(0, _randomUsernames.Length)]
                + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            byte[] b = new byte[16];
            r.NextBytes(b);
            bind.Password = Convert.ToHexString(b);
        }

        private static readonly string[] _randomUsernames =
        [
            "PixelTitan2037",
            "CaptainLovegood_",
            "TechnoMover2048",
            "PirateSamurai_",
            "VelvetPirate_",
            "SushiGazer_",
            "SilverHawk_",
            "MountainDancer_",
            "CrimsonWolf2048",
            "ElectricExplorer_",
            "CrimsonRed_",
            "EnigmaAdventure_",
            "RogueTitan2038",
            "MelodyEagle_",
            "EnigmaExplorer_",
            "LunaSoul_",
            "CaptainMaster_",
            "TangoVortex_",
            "MountainGazer_",
            "ZenMistress_",
            "BalletWillow_",
            "QuantumNebula2048",
            "EchoLullaby_",
            "SushiTornado2048",
            "SushiComet2048",
            "WhisperingLovegood_",
            "TechnoSymphony_",
            "StardustMystic_",
            "BalletCrafter_",
            "CyberSeeker_",
            "JazzHawk_",
            "JazzMystic_",
            "CaptainFire_",
            "ElectricTrance_",
            "BalletStrider_",
            "QuasarLullaby_",
            "StardustAdventure_",
            "PixelPirate_",
            "AlphaComet_",
            "RubyAdventure_",
            "JazzRed2048",
            "CyberDynamo_",
            "JazzTornado_",
            "MidnightMover2048",
            "EmeraldMystic_",
            "PixelMystic_",
            "RogueAdventure_",
            "MelodyMaster_",
            "SongbirdDancer_",
            "HarmonyStrider_",
        ];

        private void EnterButton_Click(object sender, EventArgs e)
        {
            var aaBytes = Properties.Resources.AaaScream;
            using MemoryStream ms = new MemoryStream();
            aaBytes.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Resources.Icon;
            SoundPlayer player = new SoundPlayer(ms);
            player.Play();
        }
    }
}
