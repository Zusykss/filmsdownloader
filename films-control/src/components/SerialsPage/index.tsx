import { useEffect, useState } from "react";
import {
    IGetSerialResponse,
  IPaginationFilterModel,
  IPlatformSerial,
  ISerial,
} from "./types";
import http from "../../http_common";
import Pagination from "../pagination";
import Moment from "moment";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { faSortUp } from "@fortawesome/free-solid-svg-icons";
import { faSortDown } from "@fortawesome/free-solid-svg-icons";
import { Spinner } from "react-bootstrap";
import PlatformSelector from "../PlatformSelector";
import { IStatus } from "../MoviesPage/types";
import NotesSerialModal from "../NotesSerialModal";
import SerialSelect from "../SerialSelect";
import SerialCheckUpdated from "../SerialCheckUpdated";

const SerialsPage = () => {
  const paginationFilterModel: IPaginationFilterModel = {
    pageNumber: 1,
    pageSize: 20,
  };
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isAscending, setIsAscending] = useState<boolean>(true);
  const [sortProperty, setSortProperty] = useState<string>('Id');
  const getSerialsToState = () => {
    setIsLoading(true);
    http.get<IStatus[]>("Status/getStatuses").then((data)=> {
      setStatuses(data.data);
    });
    let url: string =
      "Serial/getSerials?PageNumber=" +
      currentPage +
      "&PageSize=" +
      paginationFilterModel.pageSize;
    if (querySearch && querySearch !== "") {
      url += "&QuerySearch=" + querySearch;
    }
    url+="&OrderProperty=" + sortProperty;
    if(!isAscending){
      url+="&OrderBy=desc";
    }
    if(platforms && platforms.length > 0){
      url += `\&${platforms.map((p : number, index : number) => `platforms=${p}`).join('&')}`;
    }
    http
      .get<IGetSerialResponse>(url)
      .then((data) => {
        console.log(data);
        setSerials(data.data.items);
        setCountPages(data.data.metadata.totalPages);
      })
      .catch((err) => {
        alert(err);
      }).finally(() => {
        setIsLoading(false);
      });
  };
  const [serials, setSerials] = useState<ISerial[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1); // first page
  const [countPages, setCountPages] = useState<number>(0);
  const [querySearch, setQuerySearch] = useState<string | undefined>(undefined);
  const [statuses, setStatuses] = useState<IStatus[]>();
  const [platforms, setPlatforms] = useState<number[]>();
  useEffect(() => {
    try{
      getSerialsToState();
    }
    catch(ex){

    }
    finally{
      setIsLoading(false);
    }
  }, []);
  useEffect(() => {
    getSerialsToState();
  }, [currentPage]);
  const setPlatformsFromModal = (platforms : number[]) => {
    setPlatforms(platforms);
  }
  const applyPlatforms = () =>{
    getSerialsToState();
  }
  const applySearch = () =>{
      getSerialsToState();
  }
  return (
    <>
    <div className="p-3 d-flex align-items-end">
      <div>
        <label htmlFor="search">Search:</label>
        <input
          type="text"
          name="search"
          id="search"
          className="form-control"
          value={querySearch}
          onChange={(ev) => 
            setQuerySearch(ev.target.value)
          }
        />
        
      </div>
      
      <button
        className="btn btn-primary mx-2 px-3"
        onClick={() => {
          getSerialsToState();
        }}
      >
        Search
      </button>
      <button
        className="btn btn-danger"
        onClick={() => {
          setQuerySearch("");
          applySearch();
        }}
        >
        {" "}
        <FontAwesomeIcon icon={faCircleXmark} />
      </button>
    </div>
      <div className="p-3 d-flex mb-3 align-items-end gap-2">
        <div>

        <label htmlFor="sortSelect">Select Sort</label> 
        <select name="sortSelect" id="" className="form-control form-select" value={sortProperty} onChange={(ev) => setSortProperty(ev.target.value)}>
          <option value="Id">Id</option>
          <option value="Name">Name</option>
          <option value="ParseTime">ParseTime</option>
        </select>
        </div>
        <button className="btn btn-primary" onClick={() => getSerialsToState()}>
          Sort
        </button>
        <button className="btn btn-secondary" onClick={() => setIsAscending(!isAscending)}>
          {
            isAscending ? (
              <FontAwesomeIcon icon={faSortUp}/>
              ) : (
                <FontAwesomeIcon icon={faSortDown}/>
              )
          }
        </button>
      </div>
    
        <PlatformSelector setPlatformsFromModal={setPlatformsFromModal} applyPlatforms={applyPlatforms}></PlatformSelector>
     { isLoading ? <Spinner animation="border" className="p-5 text-center"></Spinner> : (
      <>
      
        {/* //<button></button> */}
    {serials.length > 0 || !serials ? (
      <>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Id</th>
              <th scope="col">Name</th>
              <th scope="col">Url</th>
              <th scope="col">Notes</th>
              <th scope="col">Status</th>
              <th scope="col">Platforms</th>
              <th scope="col">ParseDate</th>
              <th scope="col">Upd</th>
              <th scope="col">Episodes</th>
            </tr>
          </thead>
          <tbody>
            {serials.map((serial: ISerial) => (
              <>
                <tr>
                  <th scope="row">{serial.id}</th>
                  <td>{serial.name}</td>
                  <td>{serial.url}</td>
                  <td>
                    <NotesSerialModal serial={serial} key={serial.id}/>
                  </td>
                  <td>
                    <SerialSelect serial={serial} statuses = {statuses} key={serial.id}></SerialSelect>
                  </td>
                  <td className="table-platforms">
                    {serial.platformsSerials.some(
                      (psms: IPlatformSerial) =>
                        psms.platform.name !== "Without platform"
                    ) ? (
                        serial.platformsSerials?.map((pm: IPlatformSerial) => (
                        <img
                          src={pm.platform?.imageUrl}
                          alt={pm.platform?.name}
                        ></img>
                      ))
                    ) : (
                      <p>Without</p>
                    )}
                  </td>
                  <td>
                    {Moment(serial.parseTime).format("MM/DD/YYYY HH:mm	")}
                  </td>
                  <td>
                    <SerialCheckUpdated serial={serial} key={serial.id}></SerialCheckUpdated>
                    
                  </td>
                  <td>
                    {serial.seasons + ' ' + serial.series}
                  </td>
                </tr>
              </>
            ))}
          </tbody>
        </table>
        <Pagination
          currentPage={currentPage}
          onChangePage={(number: number) => setCurrentPage(number)}
          pageAmount={countPages}
        />
      </>
    ) : (
      <>
      <div className="d-flex justify-content-center flex-column align-items-center .bg-secondary">
        <h3 className="display-3">NOTHING!</h3>
        <p className="h3">List is empty.</p>
      </div>
        <hr />
        </>    
    )}
    </>
     )}
      
    </>
  );
};
export default SerialsPage;