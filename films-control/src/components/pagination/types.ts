export interface IPaginationMetadata {
    totalCount : number,
    pageSize : number,
    currentPage : number,
    totalPages : number,
    previousPage : string,
    nextPage : string,
    querySearch : string,
}