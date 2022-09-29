import { IPlatform } from "../home/types";
import { IPaginationMetadata } from "../pagination/types";

export interface IMovie{
    id: number,
    name: string,
    url: string,
    notes?: string,
    parseTime: Date,
    platformsMovies: IPlatformMovie[],
    status: IStatus
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
export interface IStatus{
    id: number,
    name: string
}
export default IMovie;