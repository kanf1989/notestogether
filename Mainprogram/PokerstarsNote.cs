using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mainprogram
{
    class PokerstarsNote
    {
        private PokerstarsNoteLabel[] _pokerstarsLabels;
        private PokerstarsNoteNote[] _pokerstarsNotes;

        public PokerstarsNote (string _pathToNoteFile)
        {
            String notesAsText = readNotesfile(_pathToNoteFile);

            //Einlesen der Label-Daten in die Variable conv_foreign_labels[labelcounter]
            _pokerstarsLabels = extractLabelsFromText("own", notesAsText);

            //Einlesen der Notes-Daten in die Variable conv_foreign_notes[notecounter]            
            _pokerstarsNotes = fillConvNotes("own", notesAsText);
        }

        public PokerstarsNoteLabel[] getPokerstarsLabels ()
        {
            return this._pokerstarsLabels;
        }

        public PokerstarsNoteNote[] getPokerstarsNotes()
        {
            return this._pokerstarsNotes;
        }

        public void setPokerstarsLabels(PokerstarsNoteLabel[] pokerstarsLabels)
        {
            this._pokerstarsLabels = pokerstarsLabels;
        }

        public void setPokerstarsNotes(PokerstarsNoteNote[] pokerstarsNotes)
        {
            this._pokerstarsNotes = pokerstarsNotes;
        }

        public void extractFromXml(String noteAsText)
        {

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
        private PokerstarsNoteNote[] fillConvNotes(string selection, string notesText)
        {
            int note_start = 0;
            int note_end = 0;
            string temp, temp_char;
            Boolean stop;
            int workPos, iiiiiiiii;
            PokerstarsNoteNote[] conv_notes;
            int note_iterator = 0;
            int notecounter;

            notecounter = countNotes(notesText);

            //erstellen des note arrays in Abhänigkeit von der Anzahl der Notes
            conv_notes = new PokerstarsNoteNote[notecounter];

            while ((note_start >= 0) && (note_end >= 0))
            {
                note_start = notesText.IndexOf("<note ", 0);
                note_end = notesText.IndexOf("</note>", 0);
                if ((note_start != -1) && (note_end != -1))
                {
                    conv_notes[note_iterator] = new PokerstarsNoteNote();
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

        private string readNotesfile(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }
    }
}
