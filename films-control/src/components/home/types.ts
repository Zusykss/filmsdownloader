export interface IParserStartParams{
    count?: number,
    category: number,
    platforms: number[]
}
export interface IPlatform{
    id:number,
    name: string,
    imageUrl: string,
    isSelected: boolean
} 
export default IParserStartParams;