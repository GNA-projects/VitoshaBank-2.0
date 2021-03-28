using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class BankSystemContext : DbContext
    {
        public BankSystemContext()
        {
        }

        public BankSystemContext(DbContextOptions<BankSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<ChargeAccount> ChargeAccounts { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<SupportTicket> SupportTickets { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("cards", "vitosha");

                entity.HasIndex(e => e.CardNumber, "cards_card_number_key")
                    .IsUnique();

                entity.HasIndex(e => e.ChargeAccountId, "cards_chargeaccount_id_key")
                    .IsUnique();

                entity.HasIndex(e => e.Cvv, "cards_cvv_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("card_expiration_date");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("card_number");

                entity.Property(e => e.ChargeAccountId).HasColumnName("charge_account_id");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("cvv");

                entity.HasOne(d => d.ChargeAccount)
                    .WithOne(p => p.Card)
                    .HasForeignKey<Card>(d => d.ChargeAccountId)
                    .HasConstraintName("cards_charge_accounts");
            });

            modelBuilder.Entity<ChargeAccount>(entity =>
            {
                entity.ToTable("charge_accounts", "vitosha");

                entity.HasIndex(e => e.Iban, "chargeaccounts_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('vitosha.chargeaccounts_id_seq'::regclass)");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("iban");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChargeAccounts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("charge_accounts_userid");
            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.ToTable("credits", "vitosha");

                entity.HasIndex(e => e.Iban, "credits_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.CreditAmount)
                    .HasPrecision(6, 6)
                    .HasColumnName("credit_amount");

                entity.Property(e => e.CreditAmountLeft)
                    .HasPrecision(6, 6)
                    .HasColumnName("credit_amount_left");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("iban");

                entity.Property(e => e.Instalment)
                    .HasPrecision(6, 6)
                    .HasColumnName("instalment");

                entity.Property(e => e.Interest)
                    .HasPrecision(6, 6)
                    .HasColumnName("interest");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Credits)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("credits_userid");
            });

            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.ToTable("deposits", "vitosha");

                entity.HasIndex(e => e.Iban, "deposits_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(20, 10)
                    .HasColumnName("amount");

                entity.Property(e => e.Divident)
                    .HasPrecision(20, 10)
                    .HasColumnName("divident");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("iban");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");

                entity.Property(e => e.TermOfPayment).HasColumnName("term_of_payment");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("deposits_userid");
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.ToTable("support_tickets", "vitosha");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('vitosha.supporttickets_id_seq'::regclass)");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.HasResponce).HasColumnName("has_responce");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("message");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SupportTickets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("support_user");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions", "vitosha");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("reason");

                entity.Property(e => e.RecieverAccountInfo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("reciever_account_info");

                entity.Property(e => e.SenderAccountInfo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("sender_account_info");

                entity.Property(e => e.TransactionAmount)
                    .HasPrecision(6, 6)
                    .HasColumnName("transaction_amount");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "vitosha");

                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActivationCode)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("activation_code");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("birth_date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

                entity.Property(e => e.IsConfirmed).HasColumnName("is_confirmed");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("last_name");

                entity.Property(e => e.LastTransactionId).HasColumnName("last_transaction_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("password");

                entity.Property(e => e.RegisterDate).HasColumnName("register_date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("username");

                entity.HasOne(d => d.LastTransaction)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LastTransactionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("users_transactions");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("user_accounts", "vitosha");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('vitosha.useraccounts_id_seq'::regclass)");

                entity.Property(e => e.ChargeAccountId).HasColumnName("charge_account_id");

                entity.Property(e => e.CreditId).HasColumnName("credit_id");

                entity.Property(e => e.DepositId).HasColumnName("deposit_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(60)
                    .HasColumnName("user_username");

                entity.Property(e => e.WalletId).HasColumnName("wallet_id");

                entity.HasOne(d => d.ChargeAccount)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.ChargeAccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("charge_accounts_user_accounts");

                entity.HasOne(d => d.Credit)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.CreditId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("credits_user_accounts");

                entity.HasOne(d => d.Deposit)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.DepositId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("deposits_user_accounts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAccountUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("users_user_accounts");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithMany(p => p.UserAccountUserUsernameNavigations)
                    .HasPrincipalKey(p => p.Username)
                    .HasForeignKey(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("users_username_user_accounts");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("wallets_user_accounts");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("wallets", "vitosha");

                entity.HasIndex(e => e.Amount, "wallets_amount_key")
                    .IsUnique();

                entity.HasIndex(e => e.CardNumber, "wallets_card_number_key")
                    .IsUnique();

                entity.HasIndex(e => e.Cvv, "wallets_cvv_key")
                    .IsUnique();

                entity.HasIndex(e => e.Iban, "wallets_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("card_expiration_date");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("card_number");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("cvv");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("iban");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("wallets_userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
