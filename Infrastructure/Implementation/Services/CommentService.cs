using Application.DTOs.Comment;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Comment;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class CommentService : ICommentService
	{
		private readonly IGenericRepository<Comment> _commentRepository;
		private readonly IUserIdentityService _identityService;

		public CommentService(IGenericRepository<Comment> commentRepository, IUserIdentityService identityService)
		{
			_commentRepository = commentRepository;
			_identityService = identityService;
		}

		public async Task<CommentResponseDTO> CommentPostAsync(CommentRequestDTO commentRequest)
		{
			var user = _identityService.GetLoggedInUser();

			var comment = new Comment
			{
				UserId = user.UserId,
				PostType = commentRequest.PostType,
				Content = commentRequest.Content
			};

			if (commentRequest.PostType == nameof(Blog))
			{
				comment.BlogId = commentRequest.PostId;

			}
			else if (commentRequest.PostType == nameof(Photo))
			{
				comment.PhotoId = commentRequest.PostId;
			}
			else
			{
				throw new Exception("Invalid post type.");
			}

			await _commentRepository.AddAsync(comment);

			return new CommentResponseDTO
			{
				isSuccess = true,
				PostId = commentRequest.PostId,
				PostType = comment.PostType,
				StatusMessage = "Commented Successfully"
			};

		}

		public async Task<CommentResponseDTO> EditComment(int commentId, string content)
		{
			var user = _identityService.GetLoggedInUser();

			var commentToEdit = await _commentRepository.GetByIdAsync(commentId);

			commentToEdit.Content = content;

			await _commentRepository.UpdateAsync(commentToEdit);

			return new CommentResponseDTO
			{
				isSuccess = true,
				PostId = commentToEdit.BlogId ?? commentToEdit.PhotoId,
				PostType = commentToEdit.PostType,
				StatusMessage = "Comment Edited Successfully"
			};
		}

		public async Task<TotalCommentResponseDTO> GetTotalLikes(int postId, string postType)
		{
			var totalComments = 0;

			if (postType == nameof(Blog))
			{
				totalComments = await _commentRepository.CountAsync(l => l.BlogId == postId && l.PostType == postType);
			}
			else if (postType == nameof(Photo))
			{
				totalComments = await _commentRepository.CountAsync(l => l.PhotoId == postId && l.PostType == postType);
			}
			else
			{
				throw new ArgumentException("Invalid post type.");
			}

			return new TotalCommentResponseDTO
			{
				PostId = postId,
				TotalComments = totalComments
			};
		}
	}
}
