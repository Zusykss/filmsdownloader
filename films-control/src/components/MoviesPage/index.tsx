import { useEffect, useState } from "react";
import {
  IGetMovieResponse,
  IMovie,
  IPaginationFilterModel,
  IPlatformMovie,
  IStatus,
} from "./types";
import axios from "axios";
import http from "../../http_common";
import Pagination from "../pagination";
import Moment from "moment";
import { IPaginationMetadata } from "../pagination/types";
import NotesMovieModal from "../NotesMovieModal";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { faSortUp } from "@fortawesome/free-solid-svg-icons";
import { faSortDown } from "@fortawesome/free-solid-svg-icons";
import MovieSelect from "../MovieSelect";
import { Spinner } from "react-bootstrap";
import PlatformSelector from "../PlatformSelector";

const MoviesPage = () => {
  const paginationFilterModel: IPaginationFilterModel = {
    pageNumber: 1,
    pageSize: 20,
  };
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isAscending, setIsAscending] = useState<boolean>(true);
  const [sortProperty, setSortProperty] = useState<string>('Id');
  const getMoviesToState = () => {
    setIsLoading(true);
    http.get<IStatus[]>("Status/getStatuses").then((data)=> {
      setStatuses(data.data);
    });
    let url: string =
      "Movie/getMovies?PageNumber=" +
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
      //url+=`${platforms.map((n, index) => `platforms[${index}]=${n}`).join('&')}`;
    }
    http
      .get<IGetMovieResponse>(url)
      .then((data) => {
        //console.log(data);
        setMovies(data.data.items);
        //setCurrentPage(data.data.metadata.currentPage);
        setCountPages(data.data.metadata.totalPages);
      })
      .catch((err) => {
        alert(err);
      }).finally(() => {
        setIsLoading(false);
      });
  };
  const [movies, setMovies] = useState<IMovie[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1); // first page
  const [countPages, setCountPages] = useState<number>(0);
  const [querySearch, setQuerySearch] = useState<string | undefined>(undefined);
  const [statuses, setStatuses] = useState<IStatus[]>();
  //Modal
  const [platforms, setPlatforms] = useState<number[]>();
  //const [showNotes, setShowNotes] = useState<boolean>(false);
  useEffect(() => {
    try{
      //console.log('loading');
      //setIsLoading(true);
      getMoviesToState();
    }
    catch(ex){

    }
    finally{
      setIsLoading(false);
    }
  }, []);
  useEffect(() => {
    //console.log('page');
    getMoviesToState();
  }, [currentPage]);
  // useEffect(() => {
  //   console.log('querySearch');
  //   if (querySearch === "" || !querySearch) {
  //     getMoviesToState();
  //   }
  // }, [querySearch, setQuerySearch]);
  const setPlatformsFromModal = (platforms : number[]) => {
    //console.log(platforms + ' : platforms');
    setPlatforms(platforms);
  }
  const applyPlatforms = () =>{
    //console.log('p: ' + platforms);
    getMoviesToState();
  }
  const applySearch = () =>{
    //if (querySearch === "" || !querySearch) {
      getMoviesToState();
    //}
  }
  //..сщт
  // useEffect(() => {
  //   getMoviesToState();
  // }, [setPlatformsFromModal])
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
          getMoviesToState();
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
        <button className="btn btn-primary" onClick={() => getMoviesToState()}>
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
    {movies.length > 0 || !movies ? (
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
            </tr>
          </thead>
          <tbody>
            {movies.map((movie: IMovie) => (
              <>
                <tr>
                  <th scope="row">{movie.id}</th>
                  <td>{movie.name}</td>
                  <td>{movie.url}</td>
                  <td>
                    <NotesMovieModal movie={movie} key={movie.id}/>
                  </td>
                  <td>
                    <MovieSelect movie={movie} statuses = {statuses} key={movie.id}></MovieSelect>
                  </td>
                  <td className="table-platforms">
                    {movie.platformsMovies.some(
                      (psms: IPlatformMovie) =>
                        psms.platform.name !== "Without platform"
                    ) ? (
                      movie.platformsMovies?.map((pm: IPlatformMovie) => (
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
                    {Moment(movie.parseTime).format("MM/DD/YYYY HH:mm	")}
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
export default MoviesPage;
