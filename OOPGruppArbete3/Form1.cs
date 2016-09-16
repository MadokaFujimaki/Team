using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace OOPGruppArbete3
{
    public partial class Form1 : Form
    {
        private List<ClassRoom> _classRooms  = new List<ClassRoom>();
        private List<Classes> _classes  = new List<Classes>();
        private List<Booking> _bookings = new List<Booking>();

        public Form1()
        {
            InitializeComponent();
            
            _classRooms.Add(new ClassRoom() { Name = "A101" });
            _classRooms.Add(new ClassRoom() { Name = "B403" });
            _classRooms.Add(new ClassRoom() { Name = "C206" });
            
            _classes.Add(new Classes() { Name = "C#" });
            _classes.Add(new Classes() { Name = "Frontend" });
            _classes.Add(new Classes() { Name = "Databasteknik" });

            listBox1.DisplayMember = "Display";

            ReadXmlFile();
        }

        private void ReadXmlFile()
        {
            // Create an XML reader for this file.
            using (XmlReader reader = XmlReader.Create("bookings.xml"))
            {
                Booking booking = new Booking();

                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "Bookings":
                                break;
                            case "Booking":

                                if (reader.IsStartElement())
                                {
                                    booking = new Booking();
                                }
                                else
                                {
                                    
                                }
                                
                                break;
                            case "Date":
                                try
                                {
                                    if (reader.Read())
                                    {
                                        booking.BookingDate = Convert.ToDateTime(reader.Value.Trim());

                                    }

                                }
                                catch (FormatException e)
                                {
                                    MessageBox.Show(e.Message + ": " + reader.Value.Trim());
                                }

                                break;
                            case "Room":
                                if (reader.Read())
                                {
                                    booking.Room = reader.Value.Trim();
                                }
                                break;
                            case "Class":
                                if (reader.Read())
                                {
                                    booking.Class = reader.Value.Trim();
                                }
                                break;
                        }
                        
                    }
                    else if(reader.Name == "Booking")
                    {
                        //MessageBox.Show(": " + booking.BookingDate);
                        _bookings.Add(booking);
                    }
                    
                }

                listBox1.DataSource = _bookings;

            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newMDIChild = new Form2();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ActiveMdiChild.Close();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ButtonBook_Click(object sender, EventArgs e)
        {
            var roomName = comboBox1.Text;
            var className = comboBox2.Text;
            var date = monthCalendar1.SelectionStart;

           var booking = new Booking() {Room = roomName, Class = className, BookingDate = date };

            _bookings.Add(booking);

            listBox1.DataSource = null;
            listBox1.DataSource = _bookings;
            listBox1.DisplayMember = "Display";

        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            var matches = _bookings.FindAll(
                x => x.Class.Equals(textBoxSearch.Text) ||
                        x.Room.Equals(textBoxSearch.Text));
            
            listBox1.DataSource = matches;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (XmlWriter writer = XmlWriter.Create("bookings.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Bookings");

                foreach (var booking in _bookings)
                {
                    writer.WriteStartElement("Booking");

                    writer.WriteElementString("Date", booking.BookingDate.ToString());
                    writer.WriteElementString("Room", booking.Room);
                    writer.WriteElementString("Class", booking.Class);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
