import { ChangeEvent, useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import IMovie, { IStatus } from "../MoviesPage/types";
import {faFloppyDisk} from "@fortawesome/free-solid-svg-icons";
import http from '../../http_common';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classNames from "classnames";
import { ISerial } from "../SerialsPage/types";
export interface ISerialSelectProps{
    serial: ISerial,
    statuses?: IStatus[]
}
const SerialSelect : React.FC<ISerialSelectProps> = ({serial, statuses}) =>{
    const [status, setStatus] = useState<number> (serial.status.id);
    const updateStatus = () =>{
        if(serial && statuses){
            serial.status = statuses?.find(st => st.id === status)!;
            http.post('Serial/updateStatus?id='+serial.id+'&statusId='+status);
        }
    }
    return (
      <div className={classNames(
        "d-flex",
        "gap-2",
         "justify-content-center",
         "align-items-center"
      )}>
        <select className={classNames(
            "table-select",
            "form-select",
            { "isDownloading" : status === 2}
        )}  aria-label="Default select example" value={status} onChange={(ev) => setStatus(+ev.target.value) }>
            {
                statuses?.map((st) => 
                    (
                        <option value={st.id} key={st.id}>
                            {st.name}
                        </option>
                    )
                )
            }
          
        </select>
        <button className="btn btn-primary" onClick={() => updateStatus()}>
            <FontAwesomeIcon icon={faFloppyDisk} ></FontAwesomeIcon>
        </button>
      </div>
    );
}
export default SerialSelect;