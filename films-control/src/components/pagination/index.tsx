import React from 'react';
import ReactPaginate from 'react-paginate';
import './index.css';

type PaginationProps = {
  currentPage: number;
  pageAmount: number;
  onChangePage: (page: number) => void;
};

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  onChangePage,
  pageAmount,
}) => {
  return (
    <ReactPaginate
      className='pagination'
      breakLabel='...'
      nextLabel='>'
      previousLabel='<'
      onPageChange={(event) => onChangePage(event.selected + 1)}
      pageRangeDisplayed={pageAmount}
      pageLinkClassName="page-num"
      pageCount={pageAmount}
      forcePage={currentPage - 1}
      activeLinkClassName="active"
    />
  );
};

export default Pagination;