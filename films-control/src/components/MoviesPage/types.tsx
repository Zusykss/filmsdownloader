export interface IMovie{
    id: number,
    name: string,
    url: string,
    notes?: string
}
export interface IPaginationFilterModel {
    pageNumber : number,
    pageSize : number,
    querySearch? : string
}
export default IMovie;