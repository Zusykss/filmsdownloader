import classNames from "classnames";
import { useEffect, useState } from "react";
import { Button, Modal } from "react-bootstrap";
import Swal from "sweetalert2";
import http_common from "../../http_common";
import { IPlatform } from "../home/types";

export interface IPlatformSelectorProps{
  setPlatformsFromModal : (platforms : number[]) => void;
  applyPlatforms : () => void;
}
const PlatformSelector : React.FC<IPlatformSelectorProps> = (props : IPlatformSelectorProps) => {
    const [show, setShow] = useState<boolean>(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    function clearSelections(): void {
        const newPlatforms : IPlatform[] = platforms.slice(); 
        newPlatforms.forEach((p) => p.isSelected = false);
        setPlatforms(newPlatforms);
    }
    const [platforms, setPlatforms] = useState<IPlatform[]>([]);
    const updatePlatforms = (el : IPlatform) =>{
        const newPlatforms : IPlatform[] = platforms.slice();
        const elementIndex : number | undefined = newPlatforms.findIndex(el2 => el2.id === el.id);
        if(elementIndex !== undefined){
          newPlatforms[elementIndex].isSelected = !newPlatforms[elementIndex].isSelected;
          setPlatforms(newPlatforms);
        }
      }
      useEffect(() => {
        http_common.get<IPlatform[]>("Platform/getAllPlatforms").then((data)=>{           
            setPlatforms(data.data);
        }).catch(ex => {
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!",
          });
        });
        //console.log('use');
    },[]);
    useEffect(() => {
      props.setPlatformsFromModal(platforms.filter(p => p.isSelected === true).map(p => p.id));
      //console.log('dd');
    }, [platforms, setPlatforms])
    return(
        <>
        <button onClick={() => handleShow()} className="btn btn-primary">Select platforms</button>
        <Modal
        show={show}
        onHide={handleClose}
        backdrop="static"
        keyboard={false}
      >
      <Modal.Header closeButton>
        <Modal.Title>Select platforms</Modal.Title>
      </Modal.Header>

      <Modal.Body>
        <div className="mb-2">
            <button className="btn btn-danger" onClick={() => clearSelections()}>Clear selection</button>
        </div>
        {
            platforms.map((el) => (
                <>
                <img 
                key = {el.id}
                src={el?.imageUrl} 
                // el.id !== 1 ? el?.imageUrl : ''
                alt={el?.name} 
                className={classNames(
                    "modal-platform-img",
                    {"selectedImage" : el.isSelected}
                )}
                // className={el.isSelected ? "selectedImage" : "nonSelected"}
                onClick={() => {updatePlatforms(el); }} //console.log(el.isSelected)
                ></img>
                </>
            ))
        }
      </Modal.Body>

      <Modal.Footer>
        <Button variant="primary" onClick={handleClose}>Close</Button>
        <Button variant="primary" onClick={() => {handleClose(); props.applyPlatforms()}}>Save changes</Button>
      </Modal.Footer>
      </Modal>
     
        </>
    )
}
export default PlatformSelector;


