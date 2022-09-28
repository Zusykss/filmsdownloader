import { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { INotesProps } from './types';
import http from '../../http_common';
const NotesMovieModal : React.FC<INotesProps> = ({movie}) => {
    const [show, setShow] = useState<boolean>(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const handleSave = () => {
      movie.notes = notes;
      http.post('Movie/editMovie', {movie})
      console.log(movie);
      handleClose();
    }
    const [notes, setNotes] = useState<string>();
    return(
        <>
        <h6 onClick={() => handleShow()}>{movie.notes ? movie.notes : 'Haven`t'}</h6>
        <Modal
        show={show}
        onHide={handleClose}
        backdrop="static"
        keyboard={false}
      >
      <Modal.Header closeButton>
        <Modal.Title>Edit Notes</Modal.Title>
      </Modal.Header>

      <Modal.Body>
        <p>Edit notes here.</p>
        <textarea name="notes" id="notes" className='form-control' onChange={(ev) => setNotes(ev.target.value)}/>
      </Modal.Body>

      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>Close</Button>
        <Button variant="primary" onClick={handleSave}>Save changes</Button>
      </Modal.Footer>
      </Modal>
     
        </>
    )
}
export default NotesMovieModal;