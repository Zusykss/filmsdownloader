//import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";
//import paginationFactory, { PaginationProvider } from 'react-bootstrap-table2-paginator';
// import BootstrapTable from "react-bootstrap-table-next";
//import paginationFactory from "react-bootstrap-table2-paginator";
import { useEffect, useState } from "react";
import { IMovie, IPaginationFilterModel } from "./types";
import axios from "axios";
import http from "../../http_common";
export interface IGetMovieResponse {
  movies: IMovie[];
}
const MoviesPage = () => {
  const paginationFilterModel: IPaginationFilterModel = {
    pageNumber: 1,
    pageSize: 20,
  };

  const getMoviesToState = () => {
    //paginationFilterModel : IPaginationFilterModel
    let url: string =
      "Movie/getMovies?PageNumber=" +
      paginationFilterModel.pageNumber +
      "&PageSize=" +
      paginationFilterModel.pageSize;
    if (paginationFilterModel.querySearch) {
      url += "&QuerySearch=" + paginationFilterModel.querySearch;
    }
    //const resp =
    http
      .get<IMovie[]>(url)
      .then((data) => {
        //console.log(data.data);
        setMovies(data.data);
      })
      .catch((err) => {
        alert(err);
      });
    //console.log(resp.data);
    //return resp.then(r => setMovies(r.data));
  };
  const [movies, setMovies] = useState<IMovie[]>([]);
  useEffect(() => {
    // async function fetchData(){
    //   setMovies(await getMovies(paginationFilterModel)); //(await (await getMovies(paginationFilterModel)).data)
    // }
    // fetchData();
    // setTimeout(() => {
    //   getMoviesToState();//paginationFilterModel
    //   alert("t");
    // }, 2000)
    let url: string =
      "Movie/getMovies?PageNumber=" +
      paginationFilterModel.pageNumber +
      "&PageSize=" +
      paginationFilterModel.pageSize;
    if (paginationFilterModel.querySearch) {
      url += "&QuerySearch=" + paginationFilterModel.querySearch;
    }
    //const resp =
    http
      .get<IMovie[]>(url)
      .then((data) => {
        //console.log(data.data);
        setMovies(data.data);
      })
      .catch((err) => {
        alert(err);
      })
      .then(() => {
        //console.log(movies);
      });
  }, []);
  // let products : IMovie[] = [
  //     {
  //         id : 1,
  //         name : "22",
  //         url: "33"
  //     }
  // ];
  // const columns = [
  //     {
  //       dataField: "id",
  //       text: "Product ID",
  //       sort: true
  //     },
  //     {
  //       dataField: "name",
  //       text: "Product Name",
  //       sort: true
  //     },
  //     {
  //       dataField: "price",
  //       text: "Product Price"
  //     }
  //   ];
  return (
    <>
      <button
        onClick={() => {
          console.log(movies);
        }}
      >
        {" "}
      </button>
      {/* <BootstrapTable
        bootstrap4
        keyField="id"
        data={products}
        columns={columns}
        pagination={paginationFactory({ sizePerPage: 5 })}
      /> */}
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Id</th>
            <th scope="col">Name</th>
            <th scope="col">Url</th>
            <th scope="col">Notes</th>
            <th scope="col">Platforms</th>
          </tr>
        </thead>
        <tbody>
          {movies.map((movie: IMovie) => (
            <>
              <tr>
                <th scope="row">{movie.id}</th>
                <td>{movie.name}</td>
                <td>{movie.url}</td>
                <td>{movie.notes}</td>
              </tr>
            </>
          ))}
        </tbody>
      </table>
    </>
  );
};
export default MoviesPage;
