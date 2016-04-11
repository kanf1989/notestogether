using System;
using System.Xml;


namespace Mainprogram
{
    class NotesComparer
    {
        private PokerstarsNote _ownNote, _foreignNote;
        private PokerstarsNoteNote[] _convMergedNotes, _convFinalNotes;
        private XMLNotesWriter _xmlNotesWriter;

        public void compareNotes(string path2ownNotes, string path2foreignNotes, string path2newNotes)
        {
            int _angehaengteNotesCounter;
            _xmlNotesWriter = new XMLNotesWriter();

            openNotes(path2ownNotes, path2foreignNotes);
            _angehaengteNotesCounter = startCompare();

            //anhängen der conv_merged_notes an die conv_own_notes => als ergebniss kommen die conv_final_notes raus
            _convFinalNotes = new PokerstarsNoteNote[_ownNote.getPokerstarsNotes().Length + _angehaengteNotesCounter];
            for (int i = 0; i < _ownNote.getPokerstarsNotes().Length; ++i) {
                _convFinalNotes[i] = _ownNote.getPokerstarsNotes()[i];
            }
            for (int i = 0; i < _angehaengteNotesCounter; ++i)
            {
                _convFinalNotes[i + _ownNote.getPokerstarsNotes().Length] = _convMergedNotes[i];
            }

            //******************************************Start der Writing Engine******************************************//
            _xmlNotesWriter.writeNewNotes(_ownNote.getPokerstarsLabels(), _convFinalNotes, path2newNotes, _ownNote ,_angehaengteNotesCounter);
        }      

        private int startCompare()
        {
            string foreign_playername;
            int own_durchlauf, foreign_durchlauf;
            bool vorhanden;
            int angehaengteNotesCounter = 0;
            foreign_durchlauf = 0;
            if (_ownNote.getPokerstarsNotes().Length > _foreignNote.getPokerstarsNotes().Length)
            {
                _convMergedNotes = new PokerstarsNoteNote[_ownNote.getPokerstarsNotes().Length]; //deklaration der Merged Notes Variable Größe = Größe der own_notes
            }
            else
            {
                _convMergedNotes = new PokerstarsNoteNote[_foreignNote.getPokerstarsNotes().Length]; //deklaration der Merged Notes Variable Größe der foreign_notes
            }
            do
            {
                vorhanden = false;
                foreign_playername = _foreignNote.getPokerstarsNotes()[foreign_durchlauf].getPlayerName();
                own_durchlauf = 0;
                do
                {
                    string own_playername;
                    own_playername = _ownNote.getPokerstarsNotes()[own_durchlauf].getPlayerName();
                    if (foreign_playername == own_playername)
                    {
                        vorhanden = true;
                    }
                    own_durchlauf++;
                } while ((own_durchlauf < _ownNote.getPokerstarsNotes().Length) && (vorhanden != true));
                if (vorhanden == false)
                {
                    _convMergedNotes[angehaengteNotesCounter] = new PokerstarsNoteNote();
                    _convMergedNotes[angehaengteNotesCounter].setText(_foreignNote.getPokerstarsNotes()[foreign_durchlauf].getText());
                    _convMergedNotes[angehaengteNotesCounter].setUpdateDate(_foreignNote.getPokerstarsNotes()[foreign_durchlauf].getUpdateDate());
                    _convMergedNotes[angehaengteNotesCounter].setLabel("-1");
                    _convMergedNotes[angehaengteNotesCounter].setPlayerName(_foreignNote.getPokerstarsNotes()[foreign_durchlauf].getPlayerName());
                    angehaengteNotesCounter++;

                }
                foreign_durchlauf++;
            } while (foreign_durchlauf < _foreignNote.getPokerstarsNotes().Length);
            return angehaengteNotesCounter;
        }

        //***********************************Einlesen der Informationen aus den Dateien***********************************//
        private void openNotes(string _path2ownNotes, string _path2foreignNotes)
        {
            _ownNote = new PokerstarsNote(_path2ownNotes);
            _foreignNote = new PokerstarsNote(_path2foreignNotes);
        }               
    }
}