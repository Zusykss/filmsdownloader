import { useEffect, useState } from "react";
import {
  IGetMovieResponse,
  IMovie,
  IPaginationFilterModel,
  IPlatformMovie,
} from "./types";
import axios from "axios";
import http from "../../http_common";
import Pagination from "../pagination";
import Moment from "moment";
import { IPaginationMetadata } from "../pagination/types";
import NotesMovieModal from "../NotesMovieModal";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";

const MoviesPage = () => {
  const paginationFilterModel: IPaginationFilterModel = {
    pageNumber: 1,
    pageSize: 20,
  };
  const getMoviesToState = () => {
    let url: string =
      "Movie/getMovies?PageNumber=" +
      currentPage +
      "&PageSize=" +
      paginationFilterModel.pageSize;
    if (querySearch && querySearch !== "") {
      url += "&QuerySearch=" + querySearch;
    }
    http
      .get<IGetMovieResponse>(url)
      .then((data) => {
        console.log(data);
        setMovies(data.data.items);
        setCurrentPage(data.data.metadata.currentPage);
        setCountPages(data.data.metadata.totalPages);
      })
      .catch((err) => {
        alert(err);
      });
  };
  const [movies, setMovies] = useState<IMovie[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1); // first page
  const [countPages, setCountPages] = useState<number>(0);
  const [querySearch, setQuerySearch] = useState<string | undefined>(undefined);
  //Modal

  //const [showNotes, setShowNotes] = useState<boolean>(false);
  useEffect(() => {
    getMoviesToState();
  }, []);
  useEffect(() => {
    getMoviesToState();
  }, [currentPage]);
  useEffect(() => {
    if (querySearch === "" || !querySearch) {
      getMoviesToState();
    }
  }, [querySearch]);
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
          }}
        >
          {" "}
          <FontAwesomeIcon icon={faCircleXmark} />
        </button>
      </div>
      {movies.length > 0 || !movies ? (
        <>
          <table className="table">
            <thead>
              <tr>
                <th scope="col">Id</th>
                <th scope="col">Name</th>
                <th scope="col">Url</th>
                <th scope="col">Notes</th>
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
                      <NotesMovieModal movie={movie} />
                    </td>
                    <td className="table-platforms">
                      {movie.platformsMovies.some(
                        (psms: IPlatformMovie) =>
                          psms.platform.name !== "Without platform"
                      ) ? (
                        movie.platformsMovies?.map((pm: IPlatformMovie) => (
                          // <div>
                          //pm.platform
                          <img
                            src={pm.platform?.imageUrl}
                            alt={pm.platform?.name}
                          ></img> //alt={pm.platform.name}
                          // </div>
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
            pageAmount={countPages} //{ }//{movies?.length}
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

      {/* <div className="container"> */}

      {/* </div> */}
    </>
  );
};
export default MoviesPage;
