import { IPlatform } from "../home/types";
import { IPaginationMetadata } from "../pagination/types";

export interface IMovie{
    id: number,
    name: string,
    url: string,
    notes?: string,
    parseTime: Date,
    platformsMovies: IPlatformMovie[]
}
export interface IPaginationFilterModel {
    pageNumber : number,
    pageSize : number,
    querySearch? : string
}
export interface IGetMovieResponse {
    items: IMovie[];
    metadata: IPaginationMetadata;
}
export interface IPlatformMovie{
    url: string,
    platform: IPlatform
}
export default IMovie;