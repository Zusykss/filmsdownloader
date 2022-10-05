import { IPlatform } from "../home/types";
import { IStatus } from "../MoviesPage/types";
import { IPaginationMetadata } from "../pagination/types";

export interface ISerial{
    id: number,
    name: string,
    url: string,
    seasons: string,
    series: string,
    notes?: string,
    isUpdated: boolean,
    parseTime: Date,
    platformsSerials: IPlatformSerial[],
    status: IStatus
}
export interface IPaginationFilterModel {
    pageNumber : number,
    pageSize : number,
    querySearch? : string
}
export interface IGetSerialResponse {
    items: ISerial[];
    metadata: IPaginationMetadata;
}
export interface IPlatformSerial{
    url: string,
    platform: IPlatform
}