import { ChangeEvent, useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import IMovie, { IStatus } from "../MoviesPage/types";
import {faFloppyDisk} from "@fortawesome/free-solid-svg-icons";
//import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import http from '../../http_common';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classNames from "classnames";
export interface IMovieSelectProps{
    movie: IMovie,
    statuses?: IStatus[]
}
// useEffect
const MovieSelect : React.FC<IMovieSelectProps> = ({movie, statuses}) =>{
    const [status, setStatus] = useState<number> (movie.status.id);
    //const [movieUnit, setMovieUnit] = useState<IMovie>();
    const updateStatus = () =>{
        if(movie && statuses){
            movie.status = statuses?.find(st => st.id === status)!;
            console.log('edited!');
            http.post('Movie/updateStatus?id='+movie.id+'&statusId='+status);
        }
    }
//    useEffect(() => {
//     //setStatus(movieUnit)
//         setStatus(movie.status.id);
//        setMovieUnit(movie); 
//    }, [status, movieUnit]);
    return (
      <div className={classNames(
        "d-flex",
        "gap-2",
         "justify-content-center",
         "align-items-center"
      )}>
        {/* <Form.Select aria-label="Default select example"> */}
        <select className={classNames(
            "table-select",
            "form-select",
            { "isDownloading" : status === 2}
        )}  aria-label="Default select example" value={status} onChange={(ev) => setStatus(+ev.target.value) }>
            {/* <option>Open this select menu</option> */}
          {/* <option value="1">One</option>
          <option value="2">Two</option>
          <option value="3">Three</option> */}
            {
                statuses?.map((st) => 
                    (
                        <option value={st.id} key={st.id}>
                            {st.name}
                        </option>
                    )
                )
            }
          
        {/* </Form.Select> */}
        </select>
        <button className="btn btn-primary" onClick={() => updateStatus()}>
            <FontAwesomeIcon icon={faFloppyDisk} ></FontAwesomeIcon>
        </button>
      </div>
    );
}
export default MovieSelect;