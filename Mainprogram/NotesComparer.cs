using System;
using System.IO;
using System.Xml;


namespace Mainprogram
{
    class NotesComparer
    {
        private PokerstarsNote[] _convOwnNotes, _convForeignNotes, _convMergedNotes, _convFinalNotes;
        private PokerstarsNoteLabel[] _convOwnLabels, _convForeignLabels;
        private string _path2ownNotes, _path2foreignNotes, _path2newNotes;
        private int _angehaengteNotesCounter;

        public NotesComparer(String path2ownNotes, String path2foreignNotes, String path2newNotes)
        {
            _path2ownNotes = path2ownNotes;
            _path2foreignNotes = path2foreignNotes;
            _path2newNotes = path2newNotes;
        }

        public void compareNotes()
        {
            if (compareable(_path2ownNotes, _path2foreignNotes, _path2newNotes))
            {
                fillArrays();

                startCompare();

                //anhängen der conv_merged_notes an die conv_own_notes => als ergebniss kommen die conv_final_notes raus
                _convFinalNotes = new PokerstarsNote[_convOwnNotes.Length + _angehaengteNotesCounter];
                for (int i = 0; i < _convOwnNotes.Length; ++i) {
                    _convFinalNotes[i] = _convOwnNotes[i];
                }
                for (int i = 0; i < _angehaengteNotesCounter; ++i)
                {
                    _convFinalNotes[i + _convOwnNotes.Length] = _convMergedNotes[i];
                }

                //******************************************Start der Writing Engine******************************************//
                writeNewNotes();
            }
        }

        private Boolean compareable(String pathon, String pathfn, String TextBoxPathNewNote)
        {
            return (pathon != "") && (pathfn != "") && (TextBoxPathNewNote != "");
        }

        //errechnen wieviele Labels vorhanden sind
        private int countLabels(String notes)
        {
            int labelsearcher = 0;
            int labelcounter = 0;
            while (notes.IndexOf("</label>", labelsearcher) != -1)
            {
                labelsearcher = (notes.IndexOf("</label>", labelsearcher) + 2);
                labelcounter++;
            }
            return labelcounter;
        }

        //errechnen wieviele Notes vorhanden sind
        private int countNotes(String notes)
        {
            int notecounter = 0;
            int notesearcher = 0;
            while (notes.IndexOf("</note>", notesearcher) != -1)
            {
                notesearcher = (notes.IndexOf("</note>", notesearcher) + 2);
                notecounter++;
            }

            return notecounter;
        }

        private void writeNewNotes()
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
            for (int i = 0; i < _convOwnLabels.Length; i++)
            {
                xmlTextWriter.WriteStartElement("label");
                xmlTextWriter.WriteAttributeString("id", _convOwnLabels[i].getId());
                xmlTextWriter.WriteAttributeString("color", _convOwnLabels[i].getColor());
                xmlTextWriter.WriteString(_convOwnLabels[i].getLabelName());
                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndElement();
            //Ende der labels

            //Beginn der Notes
            for (int i = 0; i < (_convOwnNotes.Length + _angehaengteNotesCounter); i++)
            {
                xmlTextWriter.WriteStartElement("note");
                xmlTextWriter.WriteAttributeString("label", _convFinalNotes[i].getLabel());
                xmlTextWriter.WriteAttributeString("player", _convFinalNotes[i].getPlayerName());
                if (_convFinalNotes[i].getUpdateDate() != compare_array[0]) xmlTextWriter.WriteAttributeString("update", _convFinalNotes[i].getUpdateDate());
                xmlTextWriter.WriteString(_convFinalNotes[i].getText());
                xmlTextWriter.WriteEndElement();
            }
            //Ende der Notes

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
        }

        private void startCompare()
        {
            string foreign_playername;
            int own_durchlauf, foreign_durchlauf;
            bool vorhanden;

            _angehaengteNotesCounter = 0;
            foreign_durchlauf = 0;
            if (_convOwnNotes.Length > _convForeignNotes.Length)
            {
                _convMergedNotes = new PokerstarsNote[_convOwnNotes.Length]; //deklaration der Merged Notes Variable Größe = Größe der own_notes
            }
            else
            {
                _convMergedNotes = new PokerstarsNote[_convForeignNotes.Length]; //deklaration der Merged Notes Variable Größe der foreign_notes
            }
            do
            {
                vorhanden = false;
                foreign_playername = _convForeignNotes[foreign_durchlauf].getPlayerName();
                own_durchlauf = 0;
                do
                {
                    string own_playername;
                    own_playername = _convOwnNotes[own_durchlauf].getPlayerName();
                    if (foreign_playername == own_playername)
                    {
                        vorhanden = true;
                    }
                    own_durchlauf++;
                } while ((own_durchlauf < _convOwnNotes.Length) && (vorhanden != true));
                if (vorhanden == false)
                {
                    _convMergedNotes[_angehaengteNotesCounter] = new PokerstarsNote();
                    _convMergedNotes[_angehaengteNotesCounter].setText(_convForeignNotes[foreign_durchlauf].getText());
                    _convMergedNotes[_angehaengteNotesCounter].setUpdateDate(_convForeignNotes[foreign_durchlauf].getUpdateDate());
                    _convMergedNotes[_angehaengteNotesCounter].setLabel("-1");
                    _convMergedNotes[_angehaengteNotesCounter].setPlayerName(_convForeignNotes[foreign_durchlauf].getPlayerName());
                    _angehaengteNotesCounter++;

                }
                foreign_durchlauf++;
            } while (foreign_durchlauf < _convForeignNotes.Length);
        }

        private string readNotesfile(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }

        //***********************************Einlesen der Informationen aus den Dateien***********************************//
        private void fillArrays()
        {
            string ownNotesAsText, foreignNotesAsText;

            ownNotesAsText = readNotesfile(_path2ownNotes);

            //Einlesen der Label-Daten in die Variable conv_foreign_labels[labelcounter]
            _convOwnLabels = extractLabelsFromText("own", ownNotesAsText);

            //Einlesen der Notes-Daten in die Variable conv_foreign_notes[notecounter]
            _convOwnNotes = fillConvNotes("own", ownNotesAsText);

            //******************************************Alle Informationen der Foreign Notes und Labels werden eingelesen******************************************///
            foreignNotesAsText = readNotesfile(_path2foreignNotes);

            //Einlesen der Label-Daten in die Variable conv_foreign_labels[labelcounter, 3]
            _convForeignLabels = extractLabelsFromText("foreign", foreignNotesAsText);
            //Ende des Einlesen Variable conv_foreign_labels[labelcounter,3]

            //Einlesen der Notes-Daten in die Variable conv_foreign_notes[notecounter, 3]
            _convForeignNotes = fillConvNotes("foreign", foreignNotesAsText);
        }

        /*
        * @param selection - can be own or foreign
        */
        private PokerstarsNoteLabel[] extractLabelsFromText(string selection, string notesText)
        {
            int label_start = 0;
            int label_end = 0;
            int labelcounter;
            string labelAsText;
            PokerstarsNoteLabel[] conv_labels;

            labelcounter = countLabels(notesText);

            //erstellen des conv_foreign_labels arrays in Abhänigkeit von der Anzahl der Labels
            conv_labels = new PokerstarsNoteLabel[labelcounter];
            for (int labelIterator = 0; labelIterator < labelcounter; ++labelIterator)
            {
                label_start = notesText.IndexOf("<label ", label_end);
                label_end = notesText.IndexOf("</label>", label_start);

                labelAsText = notesText.Substring(label_start, calcLabelLength(label_start, label_end));

                conv_labels[labelIterator] = new PokerstarsNoteLabel(labelAsText);                
            }

            return conv_labels;
        }

        /*
        * @param selection - can be own or foreign
        */
        private PokerstarsNote[] fillConvNotes(string selection, string notesText)
        {
            int note_start = 0;
            int note_end = 0;
            string temp, temp_char;
            Boolean stop;
            int workPos, iiiiiiiii;
            PokerstarsNote[] conv_notes;
            int note_iterator = 0;
            int notecounter;

            notecounter = countNotes(notesText);

            //erstellen des note arrays in Abhänigkeit von der Anzahl der Notes
            conv_notes = new PokerstarsNote[notecounter];

            while ((note_start >= 0) && (note_end >= 0))
            {
                note_start = notesText.IndexOf("<note ", 0);
                note_end = notesText.IndexOf("</note>", 0);
                if ((note_start != -1) && (note_end != -1))
                {
                    conv_notes[note_iterator] = new PokerstarsNote();
                    temp = notesText.Substring(note_start, ((note_end - note_start) + 8));
                    iiiiiiiii = 0;
                    workPos = 6;
                    stop = false;
                    while ((temp.Substring(workPos, 3) == "upd") || (temp.Substring(workPos, 3) == "lab") || (temp.Substring(workPos, 3) == "pla") || ((temp.Substring((workPos - 2), 2) == "\">") && (iiiiiiiii == 2)))
                    {
                        stop = false;
                        switch (temp.Substring(workPos, 3))
                        {
                            //einlesen des update-datums der note
                            case "upd":
                                workPos += 8;
                                conv_notes[note_iterator].setUpdateDate(temp.Substring(workPos, 10)); //einlesen des letzten update-datums
                                workPos += 13;
                                break;
                            //einlesen der label-id der note
                            case "lab":
                                workPos += 7;
                                do
                                {
                                    temp_char = temp.Substring(workPos, 1);
                                    if (temp_char == "\"") stop = true;
                                    else conv_notes[note_iterator].setLabel(conv_notes[note_iterator].getLabel() + temp_char);
                                    workPos++;
                                }
                                while (stop == false);
                                iiiiiiiii++;
                                workPos++;
                                break;
                            //einlesen des player-name
                            case "pla":
                                workPos += 8;
                                do
                                {
                                    temp_char = temp.Substring(workPos, 1);
                                    if (temp_char == "\"") stop = true;
                                    else conv_notes[note_iterator].setPlayerName(conv_notes[note_iterator].getPlayerName() + temp_char);
                                    workPos++;
                                }
                                while (stop == false);
                                iiiiiiiii++;
                                workPos++;
                                break;
                            default:
                                workPos++;
                                break;
                        }
                    }
                    if (iiiiiiiii == 2) workPos--;
                    conv_notes[note_iterator].setText(conv_notes[note_iterator].getText() + temp.Substring(workPos, (temp.Length - workPos - 8))); //einlesen der eigentlichen note

                    note_iterator++;
                    notesText = notesText.Remove(note_start, ((note_end - note_start) + 7)); //löscht die aktuelle Note aus notes
                }
            }
            return conv_notes;
        }

        private int moreLabelsExists(int label_start, int label_end)
        {
            return label_end - label_start;
        }

        private int calcLabelLength(int label_start, int label_end)
        {
            return (label_end - label_start) + 8;
        }        
    }
}