using Application.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface ICommentService
	{
		Task<CommentResponseDTO> CommentPostAsync(CommentRequestDTO commentRequest);
		Task<TotalCommentResponseDTO> GetTotalLikes(int postId, string postType);
		Task<CommentResponseDTO> EditComment(int commentId, string content);
	}
}
