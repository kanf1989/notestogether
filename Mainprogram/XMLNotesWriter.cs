using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mainprogram
{
    class XMLNotesWriter
    {
        public void writeNewNotes(PokerstarsNoteLabel[] labels, PokerstarsNoteNote[] notes, string _path2newNotes, PokerstarsNote _ownNote, int _angehaengteNotesCounter)
        {
            string[] compare_array;
            compare_array = new string[1];
            XmlTextWriter xmlTextWriter = new XmlTextWriter(_path2newNotes + "\\notes.newMerged.xml", System.Text.Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartDocument(false);
            xmlTextWriter.WriteStartElement("notes");
            xmlTextWriter.WriteAttributeString("version", "1");

            //Begin der labels:
            xmlTextWriter.WriteStartElement("labels");
            for (int i = 0; i < labels.Length; i++)
            {
                xmlTextWriter.WriteStartElement("label");
                xmlTextWriter.WriteAttributeString("id", labels[i].getId());
                xmlTextWriter.WriteAttributeString("color", labels[i].getColor());
                xmlTextWriter.WriteString(labels[i].getLabelName());
                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndElement();
            //Ende der labels

            //Beginn der Notes
            for (int i = 0; i < (_ownNote.getPokerstarsNotes().Length + _angehaengteNotesCounter); i++)
            {
                xmlTextWriter.WriteStartElement("note");
                xmlTextWriter.WriteAttributeString("label", notes[i].getLabel());
                xmlTextWriter.WriteAttributeString("player", notes[i].getPlayerName());
                if (notes[i].getUpdateDate() != compare_array[0]) xmlTextWriter.WriteAttributeString("update", notes[i].getUpdateDate());
                xmlTextWriter.WriteString(notes[i].getText());
                xmlTextWriter.WriteEndElement();
            }
            //Ende der Notes

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
        }
    }
}
