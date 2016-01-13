﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer {
    class NPCForm : NotificationForm {
        public NPC npc = null;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private static Font text_font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
        public NPCForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NPCForm));
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.npcImage = new System.Windows.Forms.PictureBox();
            this.creatureName = new System.Windows.Forms.Label();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(121, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            // 
            // npcImage
            // 
            this.npcImage.BackColor = System.Drawing.Color.Transparent;
            this.npcImage.Location = new System.Drawing.Point(12, 45);
            this.npcImage.Name = "npcImage";
            this.npcImage.Size = new System.Drawing.Size(100, 98);
            this.npcImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.npcImage.TabIndex = 1;
            this.npcImage.TabStop = false;
            // 
            // creatureName
            // 
            this.creatureName.BackColor = System.Drawing.Color.Transparent;
            this.creatureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.creatureName.Location = new System.Drawing.Point(11, 146);
            this.creatureName.MaximumSize = new System.Drawing.Size(100, 28);
            this.creatureName.Name = "creatureName";
            this.creatureName.Size = new System.Drawing.Size(100, 28);
            this.creatureName.TabIndex = 2;
            this.creatureName.Text = "Rashid";
            this.creatureName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mapUpLevel
            // 
            this.mapUpLevel.Location = new System.Drawing.Point(121, 13);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 3;
            this.mapUpLevel.TabStop = false;
            // 
            // mapDownLevel
            // 
            this.mapDownLevel.Location = new System.Drawing.Point(121, 34);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 4;
            this.mapDownLevel.TabStop = false;
            // 
            // NPCForm
            // 
            this.ClientSize = new System.Drawing.Size(328, 259);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.creatureName);
            this.Controls.Add(this.npcImage);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NPCForm";
            this.Text = "NPC Form";
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            this.ResumeLayout(false);

        }

        private MapPictureBox mapBox;
        private System.Windows.Forms.PictureBox npcImage;
        private System.Windows.Forms.Label creatureName;

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        private Dictionary<Item, int> sellItems = new Dictionary<Item, int>();
        private Dictionary<Item, int> buyItems = new Dictionary<Item, int>();
        string prefix;
        private string TooltipFunction(TibiaObject obj) {
            if (obj is Item) {
                Item item = obj as Item;
                return String.Format("{0} {1} for {2} gold.", prefix, item.displayname, prefix == "Sells" ? sellItems[item] : buyItems[item]);
            } else if (obj is Spell) {
                Spell spell = obj as Spell;
                return String.Format("{0} {1} for {2} gold.", prefix, spell.name, spell.goldcost);
            }
            return "";
        }

        public override void LoadForm() {
            if (npc == null) return;
            disposableObjects.Add(npc);

            this.SuspendLayout();
            NotificationInitialize();
            npcImage.Image = npc.image;
            creatureName.Text = MainForm.ToTitle(npc.city);


            Map m = MainForm.getMap(npc.pos.z);

            mapBox.map = m;
            mapBox.mapImage = null;

            Target t = new Target();
            t.coordinate = new Coordinate(npc.pos);
            t.image = npc.image;
            t.size = 20;

            mapBox.targets.Add(t);
            mapBox.sourceWidth = mapBox.Width;
            mapBox.mapCoordinate = new Coordinate(npc.pos);
            mapBox.zCoordinate = npc.pos.z;
            mapBox.UpdateMap();

            mapBox.Click -= c_Click;

            this.mapUpLevel.Image = MainForm.mapup_image;
            this.mapUpLevel.Click -= c_Click;
            this.mapUpLevel.Click += mapUpLevel_Click;
            this.mapDownLevel.Image = MainForm.mapdown_image;
            this.mapDownLevel.Click -= c_Click;
            this.mapDownLevel.Click += mapDownLevel_Click;

            float scale = 1.0f;
            if (npc.buyItems.Count + npc.sellItems.Count > 200) {
                scale = 0.6f;
            } else if (npc.buyItems.Count + npc.sellItems.Count > 80) {
                scale = 0.75f;
            }

            int y = mapBox.Location.Y + mapBox.Size.Height + 20;
            if (npc.buyItems.Count > 0) {
                prefix = "Sells";
                Label label = new Label();
                label.Text = "Sells";
                label.Location = new Point(40, y);
                label.ForeColor = MainForm.label_text_color;
                label.BackColor = Color.Transparent;
                label.Font = text_font;
                this.Controls.Add(label);
                y += 25;

                List<TibiaObject> list = new List<TibiaObject>();
                foreach(ItemSold itemSold in npc.buyItems) {
                    Item item = MainForm.getItem(itemSold.itemid);
                    sellItems.Add(item, itemSold.price);
                    list.Add(item);
                }

                y = y + MainForm.DisplayCreatureList(this.Controls, list, 10, y, this.Size.Width - 10, 4, false, TooltipFunction, scale);
            }
            if (npc.sellItems.Count > 0) {
                prefix = "Buys";
                Label label = new Label();
                label.Text = "Buys";
                label.Location = new Point(40, y);
                label.ForeColor = MainForm.label_text_color;
                label.BackColor = Color.Transparent;
                label.Font = text_font;
                this.Controls.Add(label);
                y += 25;

                List<TibiaObject> list = new List<TibiaObject>();
                foreach (ItemSold itemSold in npc.sellItems) {
                    Item item = MainForm.getItem(itemSold.itemid);
                    buyItems.Add(item, itemSold.price);
                    list.Add(item);
                }

                y = y + MainForm.DisplayCreatureList(this.Controls, list, 10, y, this.Size.Width - 10, 4, false, TooltipFunction, scale);
            }
            foreach (Control control in this.Controls)
                if (control is PictureBox)
                    control.Click += openItemBox;
            if (npc.spellsTaught.Count > 0) {
                prefix = "Teaches";
                Label label = new Label();
                label.Text = "Teaches";
                label.Location = new Point(40, y);
                label.ForeColor = MainForm.label_text_color;
                label.BackColor = Color.Transparent;
                label.Font = text_font;
                this.Controls.Add(label);
                y += 25;
                List<Control> spellControls = new List<Control>();

                List<TibiaObject> list = new List<TibiaObject>();
                foreach (SpellTaught teach in npc.spellsTaught) {
                    Spell spell = MainForm.getSpell(teach.spellid);
                    list.Add(spell);
                }
                list = list.OrderBy(o => (o as Spell).levelrequired).ToList();
                
                y = y + MainForm.DisplayCreatureList(this.Controls, list, 10, y, this.Size.Width - 10, 4, false, TooltipFunction, 1, spellControls);
                foreach (Control control in spellControls) {
                    control.Click += openSpellBox;
                }
            }
            this.Size = new Size(this.Size.Width, y + 20);
            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private string command_start = "item" + MainForm.commandSymbol;
        void openItemBox(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand(command_start + (sender as Control).Name);
        }
        private string spell_start = "spell" + MainForm.commandSymbol;
        void openSpellBox(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand(spell_start + (sender as Control).Name);
        }

        void mapUpLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z--;
            mapBox.UpdateMap();
            base.ResetTimer();
        }

        void mapDownLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z++;
            mapBox.UpdateMap();
            base.ResetTimer();
        }
    }
}
